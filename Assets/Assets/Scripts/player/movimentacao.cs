using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class movimentacao : MonoBehaviour
{
    /*
    Corrida
       Acelerar                                                 DONE
       Desacelerar r�pido - precis�o                            DONE
   Pulo
       Pular alto ou baixo dependendo de segurar o bot�o        DONE   
       Coyte time - game feel                                   DONE
       Poder pular antes de cair no ch�o - game feel            DONE
       M�xima velocidade de queda                               DONE
       Aumentar a gravidade na queda                            DONE
    */

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    [SerializeField] private bool jumpAnim;
    // --------------------------------------------------- Variáveis de detecção de colisão
    [SerializeField] private LayerMask camadaChao;
    [SerializeField] private Transform posicaoPe;
    [SerializeField] private float raioPe = 0.5f;
    private Collider2D colisorChao;
    [SerializeField] private bool estaNoChao = false;

    //---------------------------------------------------- Variáveis de movimentao horizontal e vertical
    [SerializeField] private float axisX;
    [SerializeField] private Vector2 velocidadeAtual;
    private float velocidadeMaxima = 10;
    [SerializeField] private float velocidadeQuedaMaxima = 35;

    [SerializeField] private float aceleracaoX = 12;
    [SerializeField] private float aceleracaoY = 50;
    [SerializeField] private float desaceleracaoNoChao = 6;
    [SerializeField] private float desaceleracaoNoAr = 3;

    // ------------------------------------------------------ Variáveis de pulo e manipulacao de gravidade
    [SerializeField] private float gravidadeInicial = 10;
    [SerializeField] private float gravidade;
    [SerializeField] private float forcaDoPulo = 8;
    private int contadorPulos = 1;
    private bool botaoPuloFoiPressionado;
    [SerializeField] private float tempoCoyote = .15f;
    [SerializeField] private float contadorCoyote;
    // ------------------------------------------------------ Variáveis de fragmentos
    [SerializeField] private LayerMask camadaFragmento;
    private GerenteDeTempo gerenteDeTempo;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        gerenteDeTempo = (GerenteDeTempo)FindFirstObjectByType(typeof(GerenteDeTempo));
    }

    private void Update()   // Detecta colisões e controla o coyote time
    {
        sprite.flipX = axisX < 0 ? true : false;

        DetectarColisoes();

        if (estaNoChao) contadorCoyote = tempoCoyote;
        else contadorCoyote -= Time.deltaTime;

        anim.SetBool("run", axisX != 0);
        anim.SetBool("estaNoChao", estaNoChao);
    }

    private void FixedUpdate()
    {
        AjusteGravidade();
        Mover();
    }

    // ------------------------------------------- Funções de movimentação horizontal e input

    private void Mover()    // Movimenta o jogador de acordo com a aceleração e desaceleração
    {
        if (axisX == 0)
        {
            float desaceleracao = (estaNoChao) ? desaceleracaoNoChao : desaceleracaoNoAr;
            velocidadeAtual.x = Mathf.MoveTowards(velocidadeAtual.x, 0, desaceleracao * Time.fixedDeltaTime);
        }
        else if (rb.linearVelocityX / axisX < 0) velocidadeAtual.x = Mathf.MoveTowards(velocidadeAtual.x, -velocidadeAtual.x, desaceleracaoNoChao * 2 * Time.fixedDeltaTime);
        else velocidadeAtual.x = Mathf.MoveTowards(velocidadeAtual.x, axisX * velocidadeMaxima, aceleracaoX * Time.fixedDeltaTime);

        if (rb.linearVelocityY < 0) rb.linearVelocityY = Mathf.MoveTowards(rb.linearVelocityY, -velocidadeQuedaMaxima, aceleracaoY * Time.fixedDeltaTime);

        rb.linearVelocityX = velocidadeAtual.x;
    }

    public void MoverInput(InputAction.CallbackContext ctx)
    {
        axisX = ctx.ReadValue<Vector2>().x;
    }

    // -----------------------------------------------------   Funções de Pular e input
    public void PularInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            botaoPuloFoiPressionado = true;
            anim.SetTrigger("jump");
        }

        if (ctx.canceled)
        {
            contadorPulos--;
            botaoPuloFoiPressionado = false;
        }

        if (contadorPulos == 1 && contadorCoyote > 0) Pular();
    }

    private void Pular()
    {
        rb.linearVelocityY = 0;
        rb.AddForce(new Vector2(rb.linearVelocityX, forcaDoPulo), ForceMode2D.Impulse);
    }

    // ---------------------------------------------------Funções Auxiliares
    private void AjusteGravidade()  // Aumenta a gravidade durante a queda para melhorar o game feel
    {
        if ((rb.linearVelocityY < 0 || !botaoPuloFoiPressionado) && !estaNoChao) gravidade += 50 * Time.deltaTime;
        else gravidade = gravidadeInicial;

        rb.gravityScale = gravidade;
    }


    private void DetectarColisoes()
    {
        colisorChao = Physics2D.OverlapBox(posicaoPe.position, new Vector2(raioPe, raioPe), 0, camadaChao);
        estaNoChao = colisorChao != null ? true : false;
        if (estaNoChao) contadorPulos = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision) // Detecta colisão com fragmentos de tempo (verifica se a camada da colisão está na camada de fragmentos)
    {
        if (((1 << collision.gameObject.layer) & camadaFragmento) != 0)
        {
            gerenteDeTempo.AumentarTempoGravado(5f);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(posicaoPe.position, new Vector3(raioPe, raioPe, raioPe));
    }

}
