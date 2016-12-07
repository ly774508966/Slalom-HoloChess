﻿using System;
using System.Collections.Generic;
using Assets.Scripts.AI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager Instance { get; set; }

        public GameObject LobbyMenu;
        public GameObject ConnectMenu;
        public GameObject HostMenu;

        // monsterTypeIds keyed to initial position nodeIds
        public Dictionary<int, int> FriendlyMonsterInitialNodeIds;
        public Dictionary<int, int> EnemyMonsterInitialNodeIds;

        public Client Client;

        public Server Server;

        private bool _isHosting;

        void Start()
        {
            Instance = this;

            ConnectMenu.SetActive(false);
            HostMenu.SetActive(false);

            _isHosting = false;

            DontDestroyOnLoad(gameObject);

        }

        void Update()
        {
            if (Client != null && Client.NetClient.connection != null)
            {
                //Debug.Log("Address: " + Client.connection.address);
            }

            if (_isHosting)
            {
                Debug.Log("Port: " + NetworkServer.listenPort);
                Debug.Log("Connections: " + NetworkServer.connections.Count);
            }


        }

        public void ConnectButton()
        {
            LobbyMenu.SetActive(false);
            ConnectMenu.SetActive(true);
            HostMenu.SetActive(false);
            Debug.Log("Connect");
        }

        public void HostButton()
        {
            try
            {
                Server = gameObject.AddComponent<Server>();
                Server.Init();

                Client = gameObject.AddComponent<Client>();
                Client.InitHost(Server.IpAddress);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

            LobbyMenu.SetActive(false);
            HostMenu.SetActive(true);

            HostMenu.transform.FindChild("IpAddress").GetComponent<Text>().text = Server.IpAddress;

            ConnectMenu.SetActive(false);
            Debug.Log("Host");
        }

        public void SinglePlayerButton(int difficulty)
        {
            try
            {
                Server = gameObject.AddComponent<Server>();
                Server.InitSinglePlayer();

                GonkDroidAI gonkDroidAI = Server.gameObject.AddComponent<GonkDroidAI>();
                gonkDroidAI.Init(Server.IpAddress);

                Client = gameObject.AddComponent<Client>();
                Client.InitHost(Server.IpAddress);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

            LobbyMenu.SetActive(false);
            HostMenu.SetActive(true);

            HostMenu.transform.FindChild("IpAddress").GetComponent<Text>().text = Server.IpAddress;

            ConnectMenu.SetActive(false);
            Debug.Log("Host");
    }

        public void ConnectToServerButton()
        {
            string hostAddress = GameObject.Find("HostInput").GetComponent<InputField>().text;

            if (string.IsNullOrEmpty(hostAddress))
            {
                hostAddress = "127.0.0.1";
            }

            try
            {
                Client = gameObject.AddComponent<Client>();
                Client.Init(hostAddress, 1300);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        public void BackButton()
        {
            LobbyMenu.SetActive(true);
            ConnectMenu.SetActive(false);
            HostMenu.SetActive(false);

            if (Client != null && Client.NetClient.isConnected)
            {
                Client.NetClient.Disconnect();
                Client = null;
            }

            if (_isHosting)
            {
                _isHosting = false;
                Server.ShutDown();
                Server = null;
            }

        }

        public void StartGame()
        {
            SceneManager.LoadScene("dejarik");
        }

    }
}
