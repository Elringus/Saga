// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Actor.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The actor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using Photon.MmoDemo.Client;				

using UnityEngine;

using Camera = UnityEngine.Camera;

/// <summary>
/// The actor.
/// </summary>
public class Actor : MonoBehaviour, IActor
{
	private Vector3 correctPosition;
	private Quaternion correctRotation;

    /// <summary>
    /// The actor text offset.
    /// </summary>
    private readonly Vector3 actorTextOffset = new Vector3(0, 2.2f, 0);

	public string ID { get; set; }
	[SerializeField]
	private int _hp;
	public int HP
	{
		get { return _hp; }
		set { _hp = value; }
	}

    public int MaxHP { get; set; }

    /// <summary>
    /// The actor.
    /// </summary>
    private Item actor;

    /// <summary>
    /// The actor text.
    /// </summary>
    private GameObject actorText;
	private GUITexture actorHP;

    /// <summary>
    /// The cam height.
    /// </summary>
    private float camHeight;

    /// <summary>
    /// The color.
    /// </summary>
    private int color;

    /// <summary>
    /// The Destroy.
    /// </summary>
    public void Destroy()
    {
		Destroy(this.actorHP.gameObject);
        Destroy(this.actorText);
        Destroy(this.gameObject);
        Destroy(this);

        this.actorText = null;
    }

    /// <summary>
    /// The initialize.
    /// </summary>
    /// <param name="actor">
    /// The actor.
    /// </param>
    /// <param name="camHeight">
    /// The cam height.
    /// </param>
    public void Initialize(Item actor, float camHeight)
    {
        this.name = "Item" + actor.Id;
        this.actor = actor;
        this.camHeight = camHeight;

		this.ID = actor.Id;

        MaxHP = 25;
        this.HP = MaxHP;

		MmoEngine.I.actors.Add(ID, this);

        this.actorText = (GameObject)Instantiate(Resources.Load("ActorName"));
        this.actorText.name = "ActorText" + actor.Id;

		this.actorHP = ((GameObject)Instantiate(Resources.Load("ActorHPPrefab"))).GetComponent<GUITexture>();
		this.actorHP.name = "ActorHP" + actor.Id;

        // this.actorText.transform.localScale = new Vector3(this.actorHeight / 8f, this.actorHeight / 8f, this.actorHeight / 8f);
        this.actorText.transform.renderer.material.color = Color.white;

        this.ShowActor(false);
        //this.transform.localScale = new Vector3(1, 3f, 1);
        //this.transform.renderer.material = (Material)Resources.Load("ActorMaterial");
    }

    /// <summary>
    /// The start.
    /// </summary>
    public void Start()
    {

    }

    /// <summary>
    /// The update.
    /// </summary>
    public void Update()
    {
        if (this.actor == null || this.actor.IsVisible == false)
        {
            this.ShowActor(false);
            return;
        }

        TextMesh textMesh = (TextMesh)this.actorText.GetComponent(typeof(TextMesh));
        //// textMesh.text = string.Format("{0} ({1},{2})", actor.Name, actor.Position.X, actor.Position.Y);
        textMesh.text = this.actor.Text;

		actorHP.transform.position = Camera.main.WorldToViewportPoint(transform.position + new Vector3(0, 2));
		actorHP.pixelInset = new Rect((float)Screen.width * -193.5f / 1589, (float)Screen.height * -29 / 894, (float)Screen.width * (245 + HP * 14) / 1589, (float)Screen.height * 55 / 894);
		actorHP.transform.GetChild(0).GetComponent<GUITexture>().pixelInset = new Rect((float)Screen.width * -64 / 1589, (float)Screen.height * -29 / 894, (float)Screen.width * 126 / 1589, (float)Screen.height * 55 / 894);

		//if (this.color != this.actor.Color)
		//{
		//	byte[] colorBytes = BitConverter.GetBytes(this.actor.Color);
		//	this.color = this.actor.Color;
		//	this.SetActorColor(new Color((float)colorBytes[2] / byte.MaxValue, (float)colorBytes[1] / byte.MaxValue, (float)colorBytes[0] / byte.MaxValue));
		//}

		correctPosition = this.GetPosition(this.actor.Position);
		this.transform.position = Vector3.Lerp(this.transform.position, correctPosition, Time.deltaTime * 15);
        
        if (this.actor.Rotation != null)
        {
			correctRotation = this.GetRotation(this.actor.Rotation);
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation, correctRotation, Time.deltaTime * 15);
        }

        this.actorText.transform.position = this.transform.position + this.actorTextOffset;

        // text looking into oposite direction of camera
        Vector3 camDiff = this.actorText.transform.position - Camera.main.transform.position;
        Vector3 lookAt = this.actorText.transform.position + camDiff;
        this.actorText.transform.LookAt(lookAt);

        this.ShowActor(true);
    }

    /// <summary>
    /// The get position.
    /// </summary>
    /// <param name="pos">
    /// The pos.
    /// </param>
    /// <returns>
    /// a vector 3
    /// </returns>
    private Vector3 GetPosition(float[] pos)
    {
        float x = pos[0] / MmoEngine.PositionFactorHorizonal;
        float y = pos[1] / MmoEngine.PositionFactorVertical;
        if (pos.Length == 2)
        {
            float terrainHeight = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, y));
            return new Vector3(x, terrainHeight + 1.5f, y);
        }

        return new Vector3(x, pos[2], y);
    }

    private void HPUpdate()
    {

    }

    private Quaternion GetRotation(float[] rotationValue)
    {
        Vector3 vector = new Vector3(rotationValue[0], rotationValue[1], rotationValue[2]);
        return Quaternion.Euler(vector);
    }

    /// <summary>
    /// The set actor color.
    /// </summary>
    /// <param name="actorColor">
    /// The actor color.
    /// </param>
    private void SetActorColor(Color actorColor)
    {
        this.transform.renderer.material.color = actorColor;
    }

    /// <summary>
    /// The show actor.
    /// </summary>
    /// <param name="show">
    /// The show.
    /// </param>
    /// <returns>
    /// true if switched showing condition.
    /// </returns>
    private bool ShowActor(bool show)
    {
        if (this.GetComponentInChildren<SkinnedMeshRenderer>().enabled != show)
        {
			foreach (var skinRenderer in this.GetComponentsInChildren<SkinnedMeshRenderer>()) skinRenderer.enabled = show;
            //this.transform.renderer.enabled = show;
            this.actorText.transform.renderer.enabled = show;
            return true;
        }

        return false;
    }

    public void TakeDamage(int dmg, string text)
    {
        HP -= dmg;
    }
}