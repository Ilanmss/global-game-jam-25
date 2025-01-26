using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GerenteDeTempo : MonoBehaviour
{
    private bool estaVoltandoNoTempo = false;
    private float tempoGravado = 11f;   // Tempo que o jogador pode voltar no tempo
    private float tempoAcumulado = 0f;
    private float tempoParaVoltar = 3f; // Tempo que o jogador leva para voltar no tempo
    private float tempoVoltarAcumulado = 0f;
    private float contadorTempo = 11f; // Contador para iniciar a volta no tempo
    [SerializeField] private Rigidbody2D player;
    private static List<Vector3> positions = new List<Vector3>();

    public Slider slidertempo;

    void FixedUpdate()  // Contador diminui e se ele passa de zero e o jogador não está voltando no tempo, ele inicia a volta no tempo
    {
        if (contadorTempo > 0)
        {
            contadorTempo -= Time.fixedDeltaTime;
            slidertempo.value = contadorTempo;
        }
        else if (!estaVoltandoNoTempo)
        {
            IniciarVoltarNoTempo();
        }

        if (estaVoltandoNoTempo && tempoVoltarAcumulado < tempoParaVoltar)
        {
            VoltarNoTempo();
        }
        else if (!estaVoltandoNoTempo)
        {
            Gravar();
        }
    }   //  se ele não estiver voltando no tempo ele está gravando

    void VoltarNoTempo()    // Não entendi bem o que faz porque pedi ajuda ao gpt, mas ele parece estar voltando no tempo durante 3 segundos
    {
        if (positions.Count > 0)
        {
            tempoVoltarAcumulado += Time.fixedDeltaTime;
            float t = tempoVoltarAcumulado / tempoParaVoltar;

            int index = Mathf.FloorToInt(t * positions.Count);
            if (index >= positions.Count)
            {
                index = positions.Count - 1;
            }

            player.transform.position = positions[index];

            if (tempoVoltarAcumulado >= tempoParaVoltar)
            {
                PararDeVoltar();
            }
        }
        else
        {
            PararDeVoltar();
        }
    }

    void Gravar()   // Grava a posição do jogador a cada segundo
    {
        tempoAcumulado += Time.fixedDeltaTime;

        if (tempoAcumulado >= 1f)
        {
            if (positions.Count > Mathf.Round(tempoGravado))
            {
                positions.RemoveAt(positions.Count - 1);
            }
            positions.Insert(0, player.transform.position);
            tempoAcumulado = 0f;
        }
    }

    private void IniciarVoltarNoTempo()    // Método que Inicia a volta no tempo
    {
        estaVoltandoNoTempo = true;
        player.bodyType = RigidbodyType2D.Kinematic;
        tempoVoltarAcumulado = 0f;  // Reseta o tempo acumulado quando inicia a volta
    }

    public void PararDeVoltar()
    {
        estaVoltandoNoTempo = false;
        player.bodyType = RigidbodyType2D.Dynamic;
        tempoVoltarAcumulado = 0f;  // Reseta o tempo acumulado quando inicia a volta
        contadorTempo = tempoGravado;   // Reseta o contador para o tempo gravado assim aumentando o tempo disponível quando aumenta o tempo gravadp
        positions.Clear();
    }

    public void AumentarTempoGravado(float tempo)   // Aumenta o tempo gravado (chamado do jogador quando colide com os fragmentos de tempo)
    {
        tempoGravado += tempo;
        contadorTempo += tempo;
        slidertempo.maxValue = tempoGravado;
        slidertempo.value = contadorTempo;
    }
}