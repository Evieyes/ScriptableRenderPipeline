using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

namespace UnityEditor.Experimental.Rendering.HDPipeline
{
    internal class SerializedHDReflectionProbe : SerializedHDProbe
    {
        internal SerializedObject serializedLegacyObject;

        internal SerializedProperty renderDynamicObjects;
        internal SerializedProperty customBakedTexture;
        internal SerializedProperty timeSlicingMode;

        SerializedProperty legacyBlendDistance;
        SerializedProperty legacySize;
        //SerializedProperty legacyOffset;
        SerializedProperty legacyMode;

        internal new HDAdditionalReflectionData target { get { return serializedObject.targetObject as HDAdditionalReflectionData; } }
        internal ReflectionProbe targetLegacy { get { return serializedLegacyObject.targetObject as ReflectionProbe; } }

        public SerializedHDReflectionProbe(SerializedObject legacyProbe, SerializedObject additionalData) : base(additionalData)
        {
            serializedLegacyObject = legacyProbe;

            customBakedTexture = legacyProbe.FindProperty("m_CustomBakedTexture");
            renderDynamicObjects = legacyProbe.FindProperty("m_RenderDynamicObjects");
            timeSlicingMode = legacyProbe.FindProperty("m_TimeSlicingMode");
            legacySize = legacyProbe.FindProperty("m_BoxSize");
            //legacyOffset = legacyProbe.FindProperty("m_BoxOffset");
            resolution = legacyProbe.FindProperty("m_Resolution");
            shadowDistance = legacyProbe.FindProperty("m_ShadowDistance");
            cullingMask = legacyProbe.FindProperty("m_CullingMask");
            useOcclusionCulling = legacyProbe.FindProperty("m_UseOcclusionCulling");
            nearClip = legacyProbe.FindProperty("m_NearClip");
            farClip = legacyProbe.FindProperty("m_FarClip");
            legacyBlendDistance = legacyProbe.FindProperty("m_BlendDistance");
            legacyMode = legacyProbe.FindProperty("m_Mode");
        }

        internal override void Update()
        {
            serializedLegacyObject.Update();
            base.Update();

            // Set the legacy blend distance to 0 so the legacy culling system use the probe extent
            legacyBlendDistance.floatValue = 0;
        }

        internal override void Apply()
        {
            //sync size with legacy reflection probe
            switch(target.influenceVolume.shape)
            {
                case InfluenceShape.Box:
                    legacySize.vector3Value = influenceVolume.boxSize.vector3Value;
                    break;
                case InfluenceShape.Sphere:
                    legacySize.vector3Value = Vector3.one * influenceVolume.sphereRadius.floatValue;
                    break;
            }

            // Sync mode with legacy reflection probe
            legacyMode.intValue = mode.intValue;

            serializedLegacyObject.ApplyModifiedProperties();
            base.Apply();
            //serializedObject.ApplyModifiedProperties(); //done in base methode
        }
    }
}
