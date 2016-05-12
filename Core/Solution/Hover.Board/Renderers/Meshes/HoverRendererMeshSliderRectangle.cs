﻿using Hover.Common.Util;
using UnityEngine;

namespace Hover.Board.Renderers.Meshes {

	/*================================================================================================*/
	public class HoverRendererMeshSliderRectangle : HoverRendererMesh {
	
		[Range(0, 100)]
		public float SizeX = 10;
		
		[Range(0, 100)]
		public float SizeY = 10;

		[Range(0, 1)]
		public float UvStartY = 0;

		[Range(0, 1)]
		public float UvEndY = 1;

		public bool IsFill = false;
		public Color TrackColor = Color.gray;
		public Color FillColor = Color.white;
		
		private float vPrevSizeX;
		private float vPrevSizeY;
		private float vPrevUvStartY;
		private float vPrevUvEndY;
		private Color vPrevColor;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override bool UpdateAfterRenderer() {
			if ( !base.UpdateAfterRenderer() ) {
				return false;
			}

			UpdateColor();
			return true;
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool CreateMeshBuilderIfNeeded(bool pNewMesh) {			
			if ( !base.CreateMeshBuilderIfNeeded(pNewMesh) ) {
				return false;
			}
			
			vPrevSizeX = -1;
			vPrevColor = new Color(0, 0, 0, -1);

			UpdateAfterRenderer();
			return true;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateMesh() {
			bool settingsAreUnchanged = (
				SizeX == vPrevSizeX &&
				SizeY == vPrevSizeY && 
				UvStartY == vPrevUvStartY &&
				UvEndY == vPrevUvEndY
			);

			if ( settingsAreUnchanged ) {
				return;
			}

			MeshUtil.BuildQuadMesh(vMeshBuild, SizeX, SizeY);

			for ( int i = 0 ; i < vMeshBuild.Uvs.Length ; i++ ) {
				Vector2 uv = vMeshBuild.Uvs[i];
				uv.y = (i == 1 || i == 2 ? UvStartY : UvEndY);
				vMeshBuild.Uvs[i] = uv;
			}

			vMeshBuild.Commit();

			vPrevSizeX = SizeX;
			vPrevSizeY = SizeY;
			vPrevUvStartY = UvStartY;
			vPrevUvEndY = UvEndY;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateColor() {
			Color color = (IsFill ? FillColor : TrackColor);

			if ( color == vPrevColor ) {
				return;
			}

			vMeshBuild.CommitColors(color);

			vPrevColor = color;
		}
		
	}

}
