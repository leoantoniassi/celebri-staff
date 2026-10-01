# Apresentação — Marco 1 (EP2) · Celebri Staff

Roteiro do pitch para a turma: **12 slides, ~10 minutos**. Cada slide tem o conteúdo da tela, quem fala e a fala sugerida (ajustem com as próprias palavras).
Fonte dos dados: [`README.md`](README.md) (Projeto Ágil do Aplicativo v2.0).

**Slides:**
- [`apresentacao/APRESENTACAO_EPICO1.pdf`](apresentacao/APRESENTACAO_EPICO1.pdf): para projetar.
- [`apresentacao/slides.html`](apresentacao/slides.html): abre no navegador.
- [`apresentacao/NOTAS_DO_APRESENTADOR.md`](apresentacao/NOTAS_DO_APRESENTADOR.md): falas de cada slide.

| Quem | Papel | Slides |
| :-- | :-- | :-- |
| **Luis Felipe** | Product Owner | 1–6, 12 |
| **Leonardo** | Dev Lead | 7, 8, 10 |
| **Bruno** | UX/UI & QA | 9, 11 |

---

### Slide 1 — Capa · Luis · 20 s
**Tela:** Celebri Staff — o app da equipe do buffet no dia da festa · Equipe L2Tech · PI 5º semestre · Marco 1 (EP2).
**Fala:** "Somos a L2Tech. Vamos mostrar o Celebri Staff: de onde partimos, o que ele virou e o que entregamos no primeiro épico."

### Slide 2 — Ideia inicial · Luis · 45 s
**Tela:**
- Sistema web sob medida para **um** buffet.
- Pivô para **SaaS White Label**: cada buffet assina e usa com a sua marca → **Celebri**.
- Plano v1.0 do app: 7 histórias genéricas (login com código da empresa, escalas, portaria QR, catraca, WhatsApp, fotos, offline).

**Fala:** "Começamos com um sistema para um buffet só. Transformamos em plataforma para vários buffets, cada um com a própria marca. O app nasceu como uma lista de funções genéricas para a equipe de campo."

### Slide 3 — Refinamentos · Luis · 60 s
**Tela (antes → depois):**
- App genérico → **telas por função**: portaria, garçom, cozinha.
- QR = 1 pessoa → **QR para 1 ou N pessoas**, quantidade editável, lista opcional.
- Comanda com cobrança → **comanda para a cozinha** (buffet é tudo incluso).
- Código da empresa → **só o e-mail** + código de primeiro acesso.
- Escala simples → **função por evento**, **check-in**, **intervalo configurável**.

**Fala:** "Discutindo com o grupo, entendemos o que cada função faz no salão. Um exemplo: a comanda não é para cobrar, porque no buffet está tudo incluso; ela serve para o garçom pedir à cozinha. E no login, só o e-mail seria inseguro, então incluímos um código enviado por e-mail no primeiro acesso."

### Slide 4 — O que é o Celebri Staff · Luis · 60 s ⭐
**Tela:** *"O app que os funcionários do buffet usam durante a festa."*
| Perfil | No app |
| :-- | :-- |
| Todos | Entrar com e-mail · minhas escalas · confirmar · check-in |
| Porteiro | Contador de pessoas · QR do convite · busca por nome |
| Garçom | Mapa das mesas · comanda · alerta de alergia |
| Cozinha | Pedidos · cardápio com horários · estoque |

**Fala:** "Em uma frase: é o app da equipe do buffet no dia da festa. Cada funcionário entra com o e-mail, vê suas escalas e, no evento, abre a tela da sua função, sempre com a marca do buffet."

### Slide 5 — Escopo e limitações · Luis · 40 s
**Tela:**
- **Dentro:** funções acima + telas do painel que alimentam o app (convites, mesas, cardápio).
- **Fora:** financeiro/contratos, cobrança, iOS, Play Store.
- **Limitações hoje:** atualiza a cada ~8 s (não é tempo real), sem modo offline, código por e-mail depende de SMTP.

**Fala:** "Somos claros sobre o que não faremos neste semestre e sobre as limitações de hoje, que já estão no backlog."

### Slide 6 — Marcos e épicos · Luis · 40 s
**Tela:** linha do tempo.
- **EP2 (30/09) — Épico 1:** base, White Label, login e escalas.
- **EP3 (15/11) — Épico 2:** operação do evento homologada, adulto/criança, cronograma.
- **EF (10/12) — Épico 3:** WhatsApp, ocorrências com foto, offline, build final.

**Fala:** "Replanejamos os épicos. Adiantamos a operação do evento, então o Épico 2 passa a validar essas telas em uso real e a completar o que falta."

### Slide 7 — Épico 1: o que propusemos · Leonardo · 40 s
**Tela:**
- Estrutura do app conectada à API.
- Login com White Label (US01).
- Escalas com aceitar/recusar (US02).

**Fala:** "Para o Épico 1, o combinado era a base: app funcionando com a API, login que aplica a marca do buffet e a tela de escalas."

### Slide 8 — Épico 1: o que realizamos · Leonardo · 90 s
**Tela:** prints do app (Minhas Escalas · Portaria · Entrada do convidado) e os números:
- ✅ Login por e-mail + primeiro acesso seguro.
- ✅ Escalas com confirmar, recusar e **check-in**.
- ✅ **Adiantado:** portaria, garçom e cozinha (app + painel web).
- **256 testes** automatizados no backend · build do app sem erros · roteiro de homologação pronto.

**Fala:** "Entregamos o proposto e fomos além. As três telas de operação já funcionam ponta a ponta com a API. Temos 256 testes automatizados no servidor e um roteiro de teste manual para homologar em celulares reais."

### Slide 9 — Jornada do usuário: portaria · Bruno · 60 s
**Tela:** as etapas da jornada, com os rostinhos de sentimento.
1. A gerente cadastra o convite.
2. A família recebe o QR no WhatsApp.
3. A recepcionista lê o QR.
4. Entrada parcial.
5. Segunda leitura.
6. Contador igual para todos.

**Fala:** "Esta é a funcionalidade principal. A Marina, recepcionista, lê o QR da Família Souza. Só o pai chegou, então ela registra 1. Quando o resto chega, o app mostra que faltam 3. Se alguém esquece o celular, ela busca pelo nome."

### Slide 10 — Desafios · Leonardo · 60 s
**Tela (desafio → solução):**
- Ambiente Android no Windows (JDK, SDK, caminho com acento) → configuração fixa e documentada.
- Bugs que só apareciam rodando → testar contra API real e emulador, não só com mocks.
- Login só com e-mail inseguro → código de 6 dígitos com hash, validade e tentativas.
- Vários porteiros ao mesmo tempo → contagem atômica no banco (testada com 10 simultâneas).
- *(+ desafios de organização da equipe)*

**Fala:** "O maior aprendizado foi que compilar não basta: dois bugs graves só apareceram quando rodamos no emulador. Depois disso, todo fluxo passou a ser testado contra a API de verdade."

### Slide 11 — LGPD e próximos passos · Bruno · 50 s
**Tela:**
- **LGPD:** o buffet é controlador e a Celebri é operadora; o QR só leva um código aleatório; senha e código ficam em hash; cada buffet só vê seus dados.
- **Até o EP3:**
  1. Homologar em celulares reais.
  2. Contador adulto/criança + lotação.
  3. Cronograma com avisos.
  4. UX.
  5. SMTP.

**Fala:** "Tratamos dados de funcionários e convidados, então aplicamos minimização e segurança desde já. Até o EP3 vamos homologar em aparelhos reais e completar contador, cronograma e UX."

### Slide 12 — Encerramento · Luis · 20 s
**Tela:** "Celebri Staff — a festa inteira na palma da mão da equipe." · links dos repositórios · Perguntas?
**Fala:** "Obrigado! A documentação completa está no README do repositório. Ficamos à disposição."

---

**Dicas para o pitch:**
- Ensaiem com cronômetro. Os slides 4 e 8 são os mais importantes: o professor pediu que fique **muito claro o que é o app**.
- Se der, mostrem o app rodando no emulador por 30 s no slide 8 (login → escalas → portaria).
