using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static AcoesJogadorEventosOpcoes;


public class GameManager : MonoBehaviour
{
    public TMP_Text textoNarrativa;

    public Button[] botoes;

    private int contadorDeTurno = 0;
    private float tempoInicio;
    private EventosOpcoesEnum eventoAtual;
    
    void Start()
    {
        tempoInicio = Time.time;
        MostrarCaminho();
    }

    void MostrarBotoes(params (string texto, Action acao)[] opcoes)
    {
        for (int i = 0; i < botoes.Length; i++)
        {
            if (i < opcoes.Length)
            {
                var opcao = opcoes[i];
                botoes[i].gameObject.SetActive(true);
                botoes[i].GetComponentInChildren<TMP_Text>().text = opcao.texto;
                botoes[i].onClick.RemoveAllListeners();
                botoes[i].onClick.AddListener(() => opcao.acao());
            }
            else
            {
                botoes[i].gameObject.SetActive(false);
            }
        }
    }

    void MostrarCaminho()
    {
        textoNarrativa.text = "Escolha para onde ir:";
        MostrarBotoes(
            ("Seguir em Frente", () => EscolherCaminho(1)),
            ("Voltar", () => EscolherCaminho(2)),
            ("Ir para a Esquerda", () => EscolherCaminho(3)),
            ("Ir para a Direita", () => EscolherCaminho(4))
        );
    }

    void EscolherCaminho(int opcao)
    {
        textoNarrativa.text = ((CaminhadaOpcoesEnum)opcao) switch
        {
            CaminhadaOpcoesEnum.SEGUIR_EM_FRENTE => "Seguindo em frente",
            CaminhadaOpcoesEnum.VOLTAR => "Voltando",
            CaminhadaOpcoesEnum.IR_PARA_A_ESQUERDA => "Indo para a Esquerda",
            CaminhadaOpcoesEnum.IR_PARA_A_DIREITA => "Indo para a Direita",
            _ => "Ação inválida"
        };
        MostrarBotoes(("Continuar", SortearEvento));
    }

    void SortearEvento()
    {
        int sorteado = UnityEngine.Random.Range(1, 5); // 1 a 4
        eventoAtual = (EventosOpcoesEnum)sorteado;

        textoNarrativa.text = eventoAtual switch
        {
            EventosOpcoesEnum.ENCONTRAR_ABISMO => "Ufá! Que bom que parei antes de cair no abismo",
            EventosOpcoesEnum.ENCONTRAR_NOVO_CAMINHO => "Inacreditável! Encontrei um novo caminho.",
            EventosOpcoesEnum.ENCONTRAR_ANIMAL_FEROZ => "Acho que acabei de ver um animal feroz logo a frente.",
            EventosOpcoesEnum.ENCONTRAR_PEDRA_MISTERIOSA => "Que pedra estranha, o que será que é?",
            _ => ""
        };
        MostrarBotoes(("Continuar", MostrarAcoesDoEvento));
    }

    void MostrarAcoesDoEvento()
    {
        switch (eventoAtual)
        {
            case EventosOpcoesEnum.ENCONTRAR_ABISMO:
                textoNarrativa.text = "O que faço agora?";
                MostrarBotoes(
                    ("Tentar descer", () => AgirAbismo(AbismoEventosEnum.DESCER)),
                    ("Pular pro outro lado", () => AgirAbismo(AbismoEventosEnum.PULAR)),
                    ("Voltar por onde veio", () => AgirAbismo(AbismoEventosEnum.VOLTAR))
                );
                break;

            case EventosOpcoesEnum.ENCONTRAR_ANIMAL_FEROZ:
                textoNarrativa.text = "O que faço agora?";
                MostrarBotoes(
                    ("Correr e fugir", () => AgirAnimal(AnimalFerozEventosEnum.CORRER)),
                    ("Desviar rapidamente", () => AgirAnimal(AnimalFerozEventosEnum.DESVIAR)),
                    ("Pular o animal", () => AgirAnimal(AnimalFerozEventosEnum.PULAR)),
                    ("Voltar silenciosamente", () => AgirAnimal(AnimalFerozEventosEnum.VOLTAR_SILENCIOSAMENTE)),
                    ("Esconder-se", () => AgirAnimal(AnimalFerozEventosEnum.ESCONDER)),
                    ("Tentar capturá-lo", () => AgirAnimal(AnimalFerozEventosEnum.CAPTURAR))
                );
                break;

            case EventosOpcoesEnum.ENCONTRAR_PEDRA_MISTERIOSA:
                textoNarrativa.text = "O que faço agora?";
                MostrarBotoes(
                    ("Analisar pedra", () => AgirPedra(PedraMisteriosaEnum.ANALISAR)),
                    ("Guardar na bolsa", () => AgirPedra(PedraMisteriosaEnum.GUARDAR)),
                    ("Jogar pedra fora", () => AgirPedra(PedraMisteriosaEnum.JOGAR))
                );
                break;

            case EventosOpcoesEnum.ENCONTRAR_NOVO_CAMINHO:
                textoNarrativa.text = "Seguindo em frente com a coragem a mil." +
                                      "Esta parte da floresta ainda não tinha explorado..." +
                                      "O que será que vou encontrar seguindo em frente?";
                contadorDeTurno++;
                MostrarBotoes(("Continuar", MostrarCaminho));
                break;
        }
    }

    void AgirAbismo(AbismoEventosEnum acao)
    {
        int taxa = acao switch
        {
            AbismoEventosEnum.DESCER => UnityEngine.Random.Range(0, 30),
            AbismoEventosEnum.PULAR => UnityEngine.Random.Range(0, 40),
            AbismoEventosEnum.VOLTAR => 100,                              
            _ => 0
        };

        if (taxa < 30)
        {
            string motivo = acao == AbismoEventosEnum.DESCER
                ? "O personagem caiu tentando descer..."
                : "O personagem pulou mas, não chegou do outro lado do abismo...";
            FimDeJogo(motivo);
            return;
        }

        string mensagem = acao switch
        {
            AbismoEventosEnum.DESCER => "Quase que tudo acaba, por pouco não escorreguei e cai...",
            AbismoEventosEnum.PULAR => "Essa foi por pouco, nunca mais tento pular um abismo daquele...",
            AbismoEventosEnum.VOLTAR => "Foi a decisão certa, aquele abismo era muito profundo",
            _ => ""
        };
        ConcluirTurno(mensagem);
    }

    void AgirPedra(PedraMisteriosaEnum acao)
    {
        if (acao == PedraMisteriosaEnum.ANALISAR)
        {
            int taxa = UnityEngine.Random.Range(0, 100);
            if (taxa < 30)
            {
                FimDeJogo("O personagem foi envenenado pelo bicho venenoso em forma de pedra...");
                return;
            }
            ConcluirTurno("Tirei a sorte grande! É ouro puro!");
        }
        else if (acao == PedraMisteriosaEnum.GUARDAR)
        {
            ConcluirTurno("Realmente, é melhor guardar e analisar quando tiver ferramentas melhores.");
        }
        else
        {
            ConcluirTurno("Joguei a pedra fora e segui em frente.");
        }
    }

    void AgirAnimal(AnimalFerozEventosEnum acao)
    {
        int taxa = acao switch
        {
            AnimalFerozEventosEnum.CORRER => UnityEngine.Random.Range(0, 80),
            AnimalFerozEventosEnum.DESVIAR => UnityEngine.Random.Range(0, 60),
            AnimalFerozEventosEnum.PULAR => UnityEngine.Random.Range(0, 40),
            AnimalFerozEventosEnum.VOLTAR_SILENCIOSAMENTE => UnityEngine.Random.Range(0, 100) + 10,
            AnimalFerozEventosEnum.ESCONDER => UnityEngine.Random.Range(0, 90),
            AnimalFerozEventosEnum.CAPTURAR => UnityEngine.Random.Range(0, 40),
            _ => 0
        };

        if (taxa < 30)
        {
            FimDeJogo("O personagem foi pego pelo animal feroz!");
            return;
        }

        ConcluirTurno("Ufá! Consegui escapar, mas é bom não passar mais ali!");
    }


    void ConcluirTurno(string mensagem)
    {
        contadorDeTurno++;
        textoNarrativa.text = mensagem;
        MostrarBotoes(("Continuar", MostrarCaminho));
    }


    void FimDeJogo(string motivo)
    {
        TimeSpan tempoDecorrido = TimeSpan.FromSeconds(Time.time - tempoInicio);
        string palavraTurno = (contadorDeTurno < 2) ? "turno" : "turnos";

        textoNarrativa.text =
            $"{motivo}\n\nFim de Jogo\n\n" +
            $"Sua aventura durou {contadorDeTurno} {palavraTurno} " +
            $"({tempoDecorrido:hh\\:mm\\:ss})";

        MostrarBotoes(("Voltar ao Menu", VoltarAoMenu));
    }

    void VoltarAoMenu()
    {
        SceneManager.LoadScene("iniciar jogo");
    }
}