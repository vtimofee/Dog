using UnityEngine;
using System;

namespace SelectedEffectOutline
{
	[RequireComponent(typeof(Renderer))]
	public class OutlineFilterObj : MonoBehaviour
	{
		public enum ETriggerMethod { MouseMove = 0, MouseRightPress, MouseLeftPress };
		public ETriggerMethod m_TriggerMethod = ETriggerMethod.MouseMove;
		public bool m_Persistent = false;
		OutlineFilter m_OutlineFilter;
		bool m_IsMouseOn = false;

		public void Initialize()
		{
			m_OutlineFilter = GameObject.FindObjectOfType<OutlineFilter>();
		}
		public void Update()
		{
			if (m_TriggerMethod == ETriggerMethod.MouseRightPress)
			{
				bool on = m_IsMouseOn && Input.GetMouseButton(1);
				if (on)
					OutlineEnable();
				else
					OutlineDisable();
			}
			else if (m_TriggerMethod == ETriggerMethod.MouseLeftPress)
			{
				bool on = m_IsMouseOn && Input.GetMouseButton(0);
				if (on)
					OutlineEnable();
				else
					OutlineDisable();
			}
		}
		void OutlineEnable()
		{
			int layer = (int)Math.Log(m_OutlineFilter.m_LayerMask.value, 2f);
			if (gameObject.layer != layer)
				gameObject.layer = layer;
		}
		void OutlineDisable()
		{
			if (m_Persistent)
				return;
			int layer = (int)Math.Log(m_OutlineFilter.m_LayerMask.value, 2f);
			if (gameObject.layer == layer)
				gameObject.layer = LayerMask.NameToLayer("Default");
		}
		void OnMouseEnter()
		{
			m_IsMouseOn = true;
			if (m_TriggerMethod == ETriggerMethod.MouseMove)
				OutlineEnable();
		}
		void OnMouseExit()
		{
			m_IsMouseOn = false;
			OutlineDisable();
		}
	}
}