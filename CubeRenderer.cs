using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class CubeRenderer
{
    private GraphicsDevice _graphicsDevice;
    private BasicEffect _effect;
    private VertexBuffer _vertexBuffer;
    private IndexBuffer _indexBuffer;

    public CubeRenderer(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
        InitializeCube();
    }

    private void InitializeCube()
    {
        // Definir los vértices del cubo
        VertexPositionColor[] vertices = new VertexPositionColor[]
        {
            new(new Vector3(-1, -1, -1), Color.Green),    // 0
            new(new Vector3( 1, -1, -1), Color.Green),  // 1
            new(new Vector3( 1,  1, -1), Color.Green),   // 2
            new(new Vector3(-1,  1, -1), Color.Green), // 3
            new(new Vector3(-1, -1,  1), Color.Green),   // 4
            new(new Vector3( 1, -1,  1), Color.Green),// 5
            new(new Vector3( 1,  1,  1), Color.Green),  // 6
            new(new Vector3(-1,  1,  1), Color.Green)   // 7
        };

        // Índices para los triángulos del cubo
        short[] indices = new short[]
        {
            0, 1, 2, 0, 2, 3, // Cara trasera
            4, 5, 6, 4, 6, 7, // Cara frontal
            0, 4, 7, 0, 7, 3, // Cara izquierda
            1, 5, 6, 1, 6, 2, // Cara derecha
            3, 2, 6, 3, 6, 7, // Cara superior
            0, 1, 5, 0, 5, 4  // Cara inferior
        };

        // Crear buffers
        _vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
        _vertexBuffer.SetData(vertices);

        _indexBuffer = new IndexBuffer(_graphicsDevice, IndexElementSize.SixteenBits, indices.Length, BufferUsage.WriteOnly);
        _indexBuffer.SetData(indices);

        // Configurar efecto
        _effect = new BasicEffect(_graphicsDevice)
        {
            VertexColorEnabled = true,
            World = Matrix.Identity
        };
    }

    public void Draw(Matrix view, Matrix projection)
    {
        _graphicsDevice.SetVertexBuffer(_vertexBuffer);
        _graphicsDevice.Indices = _indexBuffer;

        _effect.View = view;
        _effect.Projection = projection;

        foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            _graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12);
        }
    }
}
