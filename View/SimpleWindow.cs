using System;
using System.Diagnostics;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using Scratch3D.Common;
using OpenTK.Mathematics;

namespace Scratch3D.View
{
	// This is where all OpenGL code will be written.
	// OpenToolkit allows for several functions to be overriden to extend functionality; this is how we'll be writing code.

	// rotate OpenTK.Mathematics.Matrix4
	// void CreateFromAxisAngle(Vector3 axis, float angle, out Matrix4 result)
	public class SimpleWindow : GameWindow
	{

		private readonly float[] _vertices =
		{
			// 左
            -1.0f, -1.0f, -1.0f, -1.0f, 0.0f, 0.0f,
            -1.0f, -1.0f, 1.0f, -1.0f, 0.0f, 0.0f,
            -1.0f, 1.0f, 1.0f, -1.0f, 0.0f, 0.0f,
			// 追加
			-1.0f, -1.0f, -1.0f, -1.0f, 0.0f, 0.0f,
            -1.0f, 1.0f, 1.0f, -1.0f, 0.0f, 0.0f,
			// 追加
            -1.0f, 1.0f, -1.0f, -1.0f, 0.0f, 0.0f,

			// 裏
            1.0f, -1.0f, -1.0f, 0.0f, 0.0f, -1.0f,
            -1.0f, -1.0f, -1.0f, 0.0f, 0.0f, -1.0f,
            -1.0f, 1.0f, -1.0f, 0.0f, 0.0f, -1.0f,
			// 追加
			1.0f, -1.0f, -1.0f, 0.0f, 0.0f, -1.0f,
            -1.0f, 1.0f, -1.0f, 0.0f, 0.0f, -1.0f,
			// 追加
            1.0f, 1.0f, -1.0f, 0.0f, 0.0f, -1.0f,

			// 下
            -1.0f, -1.0f, -1.0f, 0.0f, -1.0f, 0.0f,
            1.0f, -1.0f, -1.0f, 0.0f, -1.0f, 0.0f,
            1.0f, -1.0f, 1.0f, 0.0f, -1.0f, 0.0f,
			// 追加
			-1.0f, -1.0f, -1.0f, 0.0f, -1.0f, 0.0f,
            1.0f, -1.0f, 1.0f, 0.0f, -1.0f, 0.0f,
			// 追加
            -1.0f, -1.0f, 1.0f, 0.0f, -1.0f, 0.0f,

			// 右
            1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f,
            1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 0.0f,
            1.0f, 1.0f, -1.0f, 1.0f, 0.0f, 0.0f,
			// 追加
			1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f,
            1.0f, 1.0f, -1.0f, 1.0f, 0.0f, 0.0f,
			// 追加
            1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f,

			// 上
            -1.0f, 1.0f, -1.0f, 0.0f, 1.0f, 0.0f,
            -1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f,
            1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f,
			// 追加
			-1.0f, 1.0f, -1.0f, 0.0f, 1.0f, 0.0f,
            1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f,
			// 追加
            1.0f, 1.0f, -1.0f, 0.0f, 1.0f, 0.0f,

			// 前
            -1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f,
            1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f,
            1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f,
			// 追加
			-1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f,
            1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f,
			// 追加
            -1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f,

        };


		private readonly uint[] _indices =
		{
			0, 1, 2, 3, 4, 5,
            6, 7, 8, 9, 10, 11,
            12, 13, 14, 15, 16, 17,
            18, 19, 20, 21, 22, 23,
			24, 25, 26, 27, 28, 29,
			30, 31, 32, 33, 34, 35
        };

        int slices = 16;
        //int slices = 16 * 2;
        int stacks = 8;
        //int stacks = 8 * 2;

        //List<float[]> solidSphereVertex = new List<float[]>();
        List<float> solidSphereVertex = new List<float>();
        List<uint> solidSphereIndex = new List<uint>();

        private int _vertexBufferObject;
		private int _vertexArrayObject;
		private int _elementBufferObject;

		private Vector2 size;
		private float scale;
		private Vector2 location;

		private const int Lcount = 2;
        private Vector4[] Lpos = new Vector4[Lcount];
        private Vector3[] Lamb = new Vector3[Lcount];
        private Vector3[] Ldiff = new Vector3[Lcount];
        private Vector3[] Lspec = new Vector3[Lcount];

        private Shader _shader;

		const float PI = 3.1415926535f;

        // A simple constructor to let us set properties like window size, title, FPS, etc. on the window.
        public SimpleWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
			: base(gameWindowSettings, nativeWindowSettings)
		{

			for (int j = 0; j <= stacks; ++j)
			{
				float t = (float)j / (float)stacks;
				float y = (float)Math.Cos(PI * t);
				float r = (float)Math.Sin(PI * t);
                for (int i = 0; i <= slices; ++i)
                {
                    float s = (float)i / (float)slices;
					float z = (float)(r * Math.Cos(PI * 2 * s));
                    float x = (float)(r * Math.Sin(PI * 2 * s));
					float[] vert = { x, y, z, x, y, z };
					//solidSphereVertex.Add(vert);
					for (int k = 0; k < vert.Length; k++) solidSphereVertex.Add(vert[k]);
                }
            }

            for (int j = 0; j < stacks; ++j)
            {
				int k = ((slices + 1) * j);
                for (int i = 0; i < slices; ++i)
                {
					uint k0 = (uint)k + (uint)i;
					uint k1 = k0 + 1;
					uint k2 = k1 + (uint) slices;
					uint k3 = k2 + 1;
					// 左下の三角形
					solidSphereIndex.Add(k0);
                    solidSphereIndex.Add(k2);
                    solidSphereIndex.Add(k3);

                    // 右上の三角形
                    solidSphereIndex.Add(k0);
                    solidSphereIndex.Add(k3);
                    solidSphereIndex.Add(k1);
                }
            }

            scale = 100.0f;
			size[0] = Size.X;
			size[1] = Size.Y;
			location[0] = 0.0f;
			location[1] = 0.0f;

			Lpos[0] = (0.0f, 0.0f, 5.0f, 1.0f);
            Lpos[1] = (8.0f, 0.0f, 0.0f, 1.0f);
            Lamb[0] = (0.2f, 0.1f, 0.1f);
            Lamb[1] = (0.1f, 0.1f, 0.1f);
            Ldiff[0] = (1.0f, 0.5f, 0.5f);
            Ldiff[1] = (0.9f, 0.9f, 0.9f);
            Lspec[0] = (1.0f, 0.5f, 0.5f);
            Lspec[1] = (0.9f, 0.9f, 0.9f);

            GLFW.SetTime(0.0);
        }


		protected override void OnLoad()
		{
			base.OnLoad();

            //GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            _vertexBufferObject = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
			//GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
			float[] ary = solidSphereVertex.ToArray();
            GL.BufferData(BufferTarget.ArrayBuffer, ary.Length * sizeof(float), ary, BufferUsageHint.StaticDraw);
            /*
			for (int i = 0; i < solidSphereVertex.Count; i++)
			{
                GL.BufferData(BufferTarget.ArrayBuffer, solidSphereVertex[i].Length * sizeof(float), solidSphereVertex[i], BufferUsageHint.StaticDraw);
            }
			*/


            _elementBufferObject = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, solidSphereIndex.Count * sizeof(uint), solidSphereIndex.ToArray(), BufferUsageHint.StaticDraw);


            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

			var positionLocation = _shader.GetAttribLocation("position");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

			/*
            var colorLocation = _shader.GetAttribLocation("color");
            GL.EnableVertexAttribArray(colorLocation);
            GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
			*/

			var normalLocation = _shader.GetAttribLocation("normal");
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(normalLocation);
			

            //GL.EnableVertexAttribArray(0);
            //GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			// 背面カリングを有効にする
			GL.FrontFace(FrontFaceDirection.Ccw);
			GL.CullFace(CullFaceMode.Back);
			GL.Enable(EnableCap.CullFace);

			// デプスバッファを有効にする
			GL.ClearDepth(1.0);
			GL.DepthFunc(DepthFunction.Less);
			GL.Enable(EnableCap.DepthTest);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Use();

			float w = size[0] / scale;
			float h = size[1] / scale;

			float fovy = scale * 0.01f;
			float aspect = size[0] / size[1];

			Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(fovy, aspect, 1.0f, 10.0f);
            //Matrix4 projection = Matrix4.CreatePerspectiveOffCenter(-w, w, -h, h, 1.0f, 10.0f);
            //Matrix4 projection = Matrix4.CreateOrthographic(w, h, 1.0f, 10.0f);


            //Matrix4.CreateScale(scale / size[0], scale / size[1], 1.0f, out Matrix4 scaleMat);
            Matrix4 rotate = Matrix4.CreateFromAxisAngle(Vector3.UnitY, (float) GLFW.GetTime());
            Matrix4.CreateTranslation(location[0], location[1], 0.0f, out Matrix4 translationMat);

            //Matrix4.Mult(translationMat, scaleMat, out Matrix4 vertexModel); 

            //Matrix4 model = translationMat * scaleMat;
            Matrix4 model = translationMat * rotate;
            //Matrix4 model = translationMat;

            Vector3 eye = (3.0f, 4.0f, 5.0f);
			Vector3 target = (0.0f, 0.0f, 0.0f);
			Vector3 up = (0.0f, 1.0f, 0.0f);

			Matrix4 view = Matrix4.LookAt(eye, target, up);

            //Matrix4 vertexModel = view * model;
            Matrix4 vertexModel = model * view;

			Matrix3 normalMatrix = this.GetNormalMatrix(vertexModel);

            _shader.SetMatrix4("projection", projection);
			_shader.SetMatrix4("model", vertexModel);
			_shader.SetMatrix3("normalMatrix", normalMatrix);

			for (int i = 0; i < Lcount; i++)
			{
                _shader.SetVector4("Lpos"+(i+1), Lpos[i] * view);
                _shader.SetVector3("Lamb" + (i + 1), Lamb[i]);
                _shader.SetVector3("Ldiff" + (i + 1), Ldiff[i]);
                _shader.SetVector3("Lspec" + (i + 1), Lspec[i]);
            }
            


            //GL.BindVertexArray(_vertexArrayObject);
            //GL.DrawArrays(PrimitiveType.LineLoop, 0, 4);
            //GL.DrawArrays(PrimitiveType.LineLoop, 0, 12);
            //GL.DrawElements(PrimitiveType.Lines, _indices.Length, DrawElementsType.UnsignedInt, 0);
            //GL.DrawElements(PrimitiveType.Triangles, 0, 0, 0);

            //GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.DrawElements(PrimitiveType.Triangles, solidSphereIndex.Count, DrawElementsType.UnsignedInt, 0);

            Matrix4.CreateTranslation(0.0f, 0.0f, 3.0f, out Matrix4 translationMat2);
            Matrix4 model2 = translationMat2 * rotate;
            Matrix4 vertexModel2 = model2 * view;
            Matrix3 normalMatrix2 = this.GetNormalMatrix(vertexModel2);
            _shader.SetMatrix4("model", vertexModel2);
            _shader.SetMatrix3("normalMatrix", normalMatrix2);
            //GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.DrawElements(PrimitiveType.Triangles, solidSphereIndex.Count, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();

		}

		// This function runs on every update frame.
		protected override void OnUpdateFrame(FrameEventArgs e)
		{

			float mX = 2.0f / size[0];
			float mY = 2.0f / size[1];
			if (KeyboardState.IsKeyDown(Keys.Right))
			{
				location[0] += mX;
				if (location[0] > size[0] / scale)
				{
					location[0] = size[0] / scale;
				}
			}
			if (KeyboardState.IsKeyDown(Keys.Left))
			{
				location[0] -= mX;
				if (location[0] < -(size[0] / scale))
				{
					location[0] = -(size[0] / scale);
				}
			}
			if (KeyboardState.IsKeyDown(Keys.Up))
			{
				location[1] += mY;
				if (location[1] > size[1] / scale - 1)
				{
					location[1] = size[1] / scale - 1;
				}
			}
			if (KeyboardState.IsKeyDown(Keys.Down))
			{
				location[1] -= mY;
				if (location[1] < - (size[1] / scale - 1))
				{
					location[1] = -(size[1] / scale - 1);
				}
			}


			// Check if the Escape button is currently being pressed.
			if (KeyboardState.IsKeyDown(Keys.Escape))
			{
				// If it is, close the window.
				Close();
			}

			base.OnUpdateFrame(e);
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
			var mouse = MouseState;
			location[0] = mouse.X * 2.0f / size[0] - 1.0f; 
			location[1] = 1.0f - mouse.Y * 2.0f / size[1];
		}

		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			base.OnMouseWheel(e);
			scale += e.OffsetY;
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
			GL.Viewport(0, 0, Size.X, Size.Y);
			size[0] = Size.X;
			size[1] = Size.Y;
			//aspect = e.Width / e.Height;
			//aspect = 1.0f;
		}

		protected override void OnUnload()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
			GL.UseProgram(0);

			GL.DeleteBuffer(_vertexBufferObject);
			GL.DeleteVertexArray(_vertexArrayObject);

			GL.DeleteProgram(_shader.Handle);

			base.OnUnload();
		}


		private Matrix3 GetNormalMatrix(Matrix4 matrix)
		{
			Matrix3 result = new Matrix3();
			result.Row0.X = matrix.Row1.Y * matrix.Row2.Z - matrix.Row1.Z * matrix.Row2.Y;
            result.Row0.Y = matrix.Row1.Z * matrix.Row2.X - matrix.Row1.X * matrix.Row2.Z;
            result.Row0.Z = matrix.Row1.X * matrix.Row2.Y - matrix.Row1.Y * matrix.Row2.X;
            result.Row1.X = matrix.Row2.Y * matrix.Row0.Z - matrix.Row2.Z * matrix.Row0.Y;
            result.Row1.Y = matrix.Row2.Z * matrix.Row0.X - matrix.Row2.X * matrix.Row0.Z;
            result.Row1.Z = matrix.Row2.X * matrix.Row0.Y - matrix.Row2.Y * matrix.Row0.X;
            result.Row2.X = matrix.Row0.Y * matrix.Row1.Z - matrix.Row0.Z * matrix.Row1.Y;
            result.Row2.Y = matrix.Row0.Z * matrix.Row1.X - matrix.Row0.X * matrix.Row1.Z;
            result.Row2.Z = matrix.Row0.X * matrix.Row1.Y - matrix.Row0.Y * matrix.Row1.X;
            return result;
		}

	}
}
