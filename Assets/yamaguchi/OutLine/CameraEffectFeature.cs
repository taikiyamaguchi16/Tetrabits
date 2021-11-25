using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class CameraEffectFeature : ScriptableRendererFeature
{
    [SerializeField] private Material material;

    class CustomRenderPass : ScriptableRenderPass
    {
        public Material material;


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null)
                return;

            var camera = renderingData.cameraData.camera;

            var cmd = CommandBufferPool.Get(string.Empty);
            cmd.Blit(Texture2D.whiteTexture, camera.activeTexture, material);

            context.ExecuteCommandBuffer(cmd);
            context.Submit();
        }


    }

    CustomRenderPass m_ScriptablePass;

    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass();


        m_ScriptablePass.material = material;

        m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRendering + 2;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }
}