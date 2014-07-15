// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The player.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using Photon.MmoDemo.Client;

using UnityEngine;

using System.Collections.Generic;

/// <summary>
/// The player.
/// </summary>
public class Player : MonoBehaviour, IActor
{
    /// <summary>
    /// The change text.
    /// </summary>
    private bool changeText = false;

    private UnitOperations UOP;

	public string ID { get; set; }

    public int HP { get; set; }

    public int MaxHP { get; set; }

    private bool isDead = false;

    /// <summary>
    /// The engine.
    /// </summary>
    private Game engine;

    /// <summary>
    /// The last key press.
    /// </summary>
    private float lastKeyPress;

    /// <summary>
    /// The last move position.
    /// </summary>
    private Vector3 lastMovePosition;

    /// <summary>
    /// The last move rotation.
    /// </summary>
    private Vector3 lastMoveRotation;

    /// <summary>
    /// The next move time.
    /// </summary>
    private float nextMoveTime;

    /// <summary>
    /// The name text.
    /// </summary>
    private GUIText nameText;

    /// <summary>
    /// The view text.
    /// </summary>
    private GUIText viewText;

    /// <summary>
    /// The get position.
    /// </summary>
    /// <param name="position">
    /// The position.
    /// </param>
    /// <returns>
    /// the position as float array
    /// </returns>
    public static float[] GetPosition(Vector3 position)
    {
        float[] result = new float[3];
        result[0] = position.x * MmoEngine.PositionFactorHorizonal;
        result[1] = position.z * MmoEngine.PositionFactorVertical;
        result[2] = position.y;
        return result;
    }

    public static float[] GetRotation(Vector3 rotation)
    {
        float[] rotationValue = new float[3];
        rotationValue[0] = rotation.x;
        rotationValue[1] = rotation.y;
        rotationValue[2] = rotation.z;
        return rotationValue;
    }

    /// <summary>
    /// The initialize.
    /// </summary>
    /// <param name="engine">
    /// The engine.
    /// </param>
    public void Initialize(Game engine)
    {
		this.ID = engine.Avatar.Id;

        MaxHP = 25;
        this.HP = MaxHP;


        UOP = new UnitOperations(ID);

		MmoEngine.I.actors.Add(ID, this);
		
        this.nextMoveTime = 0;
        this.engine = engine;
        this.nameText = (GUIText)GameObject.Find("PlayerNamePrefab").GetComponent("GUIText");
        this.viewText = (GUIText)GameObject.Find("ViewDistancePrefab").GetComponent("GUIText");

		this.engine.Avatar.SetText(this.engine.Avatar.Text);
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
        try
        {
            if (this.engine != null)
            {
                this.nameText.text = this.engine.Avatar.Text;
                this.viewText.text = string.Format("{0:0} x {1:0}", this.engine.Avatar.ViewDistanceEnter[0], this.engine.Avatar.ViewDistanceEnter[1]);
                this.Move();
                this.ReadKeyboardInput();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    /// <summary>
    /// The move.
    /// </summary>
    private void Move()
    {
        if (Time.time > this.nextMoveTime)
        {
            Vector3 rotation = this.transform.rotation.eulerAngles;
            if (this.lastMovePosition != this.transform.position || this.lastMoveRotation != rotation)
            {
                this.engine.Avatar.MoveAbsolute(GetPosition(this.transform.position), GetRotation(rotation));
                this.lastMovePosition = this.transform.position;
                this.lastMoveRotation = rotation;
            }

            // up to 10 times per second
            this.nextMoveTime = Time.time + 0.1f;
        }
    }


    private void MakeAttack(int dmg)
    {
        UOP.RequestAttack(ID, dmg);
    }

    public void TakeDamage(int dmg, string text)
    {
        HP -= dmg;
    }
    /// <summary>
    /// The read keyboard input.
    /// </summary>
    private void ReadKeyboardInput()
    {
        if (this.changeText)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                this.changeText = false;
                return;
            }

            if (Input.GetKey(KeyCode.Backspace))
            {
                if (this.lastKeyPress + 0.1f < Time.time)
                {
                    if (this.engine.Avatar.Text.Length > 0)
                    {
						this.engine.Avatar.SetText(this.engine.Avatar.Text.Remove(this.engine.Avatar.Text.Length - 1));
                        this.lastKeyPress = Time.time;
                    }
                }

                return;
            }
            engine.Avatar.SetText(this.engine.Avatar.Text + Input.inputString);
            return;
        }

        if (Input.GetKey(KeyCode.F1))
        {
            this.changeText = true;
            return;
        }

        if(Input.GetKey(KeyCode.A))
        {
            MakeAttack(3);
        }

        if (Input.GetKey(KeyCode.S))
        {
            MakeAttack(5);
        }
    }
}