# Apresentação — Marco 2 (EP3) · Celebri Staff

Roteiro do pitch para a turma: **12 slides, ~11 minutos**. Cada slide tem o conteúdo da tela, quem fala e uma fala sugerida (ajustem com as próprias palavras).

Fonte dos dados: [`README.md`](README.md) (Projeto Ágil do Aplicativo v3.0).

**Slides:**
- [`apresentacao/APRESENTACAO_EPICO2.pdf`](apresentacao/APRESENTACAO_EPICO2.pdf): para projetar.
- [`apresentacao/slides-epico2.html`](apresentacao/slides-epico2.html): abre no navegador.

**Cobertura do que foi pedido:**

| Item pedido | Slide |
| :-- | :-: |
| A solução (app) proposta | 2 |
| Funcionalidades características de aplicativo | 3 |
| Review do Épico 1 | 4 |
| Descrição do Épico 2 | 5 |
| Proposto no Épico 2 | 6 |
| Realizado no Épico 2 | 7, 8 |
| Arquitetura da camada de dados *(da documentação)* | 9 |
| Desafios | 10 |
| Próximos passos até o EF | 11 |

**Divisão das falas** (sugestão; mesma divisão do Marco 1):

| Quem | Papel | Slides |
| :-- | :-- | :-- |
| **Luis Felipe** | Product Owner | 1, 2, 4, 5, 11, 12 |
| **Leonardo** | Dev Lead | 6, 8, 9, 10 |
| **Bruno** | UX/UI & QA | 3, 7 |

---

### Slide 1 — Capa · Luis · 15 s
**Tela:** Celebri Staff — o app da equipe do buffet no dia da festa · Equipe L2Tech · PI 5º semestre · Marco 2 (EP3).

**Fala:** "Somos a L2Tech. Hoje mostramos como o Celebri Staff está, o que fechamos do Épico 1 e o que fizemos no Épico 2."

### Slide 2 — A solução · Luis · 60 s
**Tela:** *"O app que os funcionários do buffet usam durante a festa, com a marca do buffet."*
- **Todos:** entrar com e-mail, minhas escalas, confirmar, check-in.
- **Porteiro:** contador compartilhado, QR do convite (1 ou N pessoas), busca por nome.
- **Garçom:** mapa das mesas, comanda para a cozinha, alerta de alergia.
- **Cozinha:** pedidos, cardápio com horários, estoque.
- *Faixa:* de "sistema para um buffet" → SaaS White Label → app por função, sem cobrança, login só com e-mail.

**Fala:** "A ideia se manteve desde o Marco 1. O painel web é do gerente e o app é da equipe no salão: cada funcionário entra com o e-mail e, no evento, abre a tela da sua função. Neste épico a ideia não mudou; o que mudou foi deixar o app pronto para ser usado de verdade."

### Slide 3 — Por que é um app, e não um site · Bruno · 60 s ⭐
**Tela (recurso → para quê):**
- 📷 **Câmera + leitor de QR nativo**, com lanterna → portaria em salão escuro.
- 📳 **Vibração** ao ler o QR e ao concluir uma ação → confirma sem olhar.
- 🔐 **Armazenamento seguro do Android** (Keystore) → sessão criptografada no aparelho.
- 🔁 **Sessão persistente** → abre direto nas escalas.
- 🌗 **Modo claro/escuro do sistema** e barra de status → legível no salão.
- 🎨 **White Label em tempo de execução** → um app, a marca de cada buffet.
- 📲 Ícone, splash, tela cheia, toasts e navegação nativos.

**Fala:** "O Celebri Staff é um app nativo Android feito em .NET MAUI, não um site no celular. A porteira aponta a câmera, o celular vibra quando lê o convite e, se o salão estiver escuro, ela liga a lanterna pelo próprio app. O login fica guardado criptografado no Android, então o funcionário abre o app e já cai nas escalas dele. E o tema segue o modo escuro do celular."

### Slide 4 — Review do Épico 1 · Luis · 60 s
**Tela:**
- **Resultados:** base do app, login por e-mail + código, escalas com check-in e, **adiantados**, portaria, garçom e cozinha. Eram 256 testes no backend.
- **Pendências → como ficaram:**
  - UX das telas → ✅ resolvida (design system).
  - SMTP do código → ✅ em dev (Mailpit).
  - LGPD: privacidade e anonimização → ✅ na API.
  - Homologação em celular real → 🔄 parcial (app no celular por USB).
  - Adulto/criança, cronograma, atalho do gerente → 📅 replanejados para o EF.

**Fala:** "No Épico 1 entregamos a base e adiantamos as telas de operação. Das pendências que deixamos, fechamos a UX, o e-mail do código em desenvolvimento e a parte de LGPD. A homologação em aparelho começou: o app já roda no celular físico. Três itens foram replanejados de forma consciente, e explico no próximo slide."

### Slide 5 — Épico 2: objetivo, escopo e limitações · Luis · 45 s
**Tela:**
- **Objetivo:** app **pronto para uma festa de verdade**.
- **Escopo:** homologação, UX, adulto/criança, cronograma, atalho do gerente, SMTP. **Incluído no caminho:** segurança e LGPD da API.
- **Limitações:** cerca de 5 semanas; QR só testa em celular com câmera; sem servidor de produção (SMTP real e HTTPS ficam para a publicação).

**Fala:** "O objetivo do Épico 2 era sair do 'funciona no emulador' para 'dá para usar numa festa'. Para isso faltavam telas claras, rodar no celular de verdade e uma API segura para dados reais de convidados."

### Slide 6 — Épico 2: o que propusemos · Leonardo · 40 s
**Tela (lista numerada):**
1. Homologar portaria, garçom e cozinha em celulares reais.
2. Contador adulto/criança com barra de lotação.
3. Cronograma do evento com avisos.
4. Atalho do gerente para os módulos.
5. Refinar a UX das telas de operação.
6. SMTP e fluxo com Pull Requests.

**Fala:** "Este foi o combinado no Marco 1: seis itens, metade de validação e acabamento, metade de funcionalidade nova."

### Slide 7 — Épico 2: o que realizamos (app) · Bruno · 75 s
**Tela:** a lista abaixo e um quadro com a paleta clara/escura e os 6 componentes. Se quiserem, troquem o quadro por 3 prints do celular (escalas, portaria e modo escuro) em `apresentacao/img/`.
- ✅ **Design system:** paleta Celebri, modo escuro, ícones, 6 componentes reutilizáveis.
- ✅ **Feedback:** toast + vibração no sucesso, aviso de erro, skeleton no carregamento.
- ✅ Fontes, logo, ícone e splash novos.
- ✅ **Celular físico** pelo cabo USB, sem IP fixo.

**Fala:** "No app, o foco foi a experiência no salão. Padronizamos todas as telas com a mesma paleta e os mesmos componentes, criamos o modo escuro e passamos a confirmar cada ação com toast e vibração. E agora o app roda no celular físico só com o cabo, sem precisar descobrir o IP do computador."

### Slide 8 — Épico 2: o que realizamos (API) e o placar · Leonardo · 60 s
**Tela:**
- ✅ **Segurança e LGPD em 8 fases:**
  - bloqueio de conta e limite de tentativas;
  - sessão de 8h revogável;
  - funcionário de campo sem acesso a rotas de gerente;
  - auditoria, exportação e anonimização de titular;
  - política de privacidade;
  - CI com CodeQL.
- **Placar do proposto:**
  - ✅ UX;
  - ✅ SMTP (dev);
  - 🔄 homologação;
  - 📅 adulto/criança, cronograma e atalho do gerente.
- **Números:** **895 testes** no backend (eram 256) · migrations 012 e 013.

**Fala:** "Na API fizemos uma auditoria de segurança em oito fases antes de colocar dados reais de convidados no app. Sendo honestos com o placar: UX e segurança estão feitos, a homologação está em andamento, e adulto/criança, cronograma e atalho do gerente não saíram neste épico. A suíte de testes do servidor foi de 256 para 895."

### Slide 9 — Camada de dados · Leonardo · 60 s
**Tela:** os 4 lugares e o diagrama do modelo local.
- 🔐 **SecureStorage** (Keystore): `auth_token` (JWT 8h) e `current_user` (JSON do usuário).
- ⚙️ **Preferences:** `tenant_slug` (buffet escolhido).
- 🧠 **Memória:** cores, logo, listas das telas.
- ☁️ **API + PostgreSQL:** escalas, convites, contador, mesas, pedidos, cardápio, estoque; atualização a cada 8 s.
- 🚫 **Sem banco local:** dados da festa são compartilhados entre funcionários; lista de convidados fora do celular (LGPD).

**Fala:** "O servidor é a fonte da verdade. No celular guardamos só o necessário para abrir já logado e com a marca certa: o token e o usuário, criptografados, e o identificador do buffet. Tudo da festa fica na API, porque dois porteiros precisam ver o mesmo total. E não deixamos lista de convidados no celular do funcionário."

### Slide 10 — Desafios · Leonardo · 50 s
**Tela (desafio → como enfrentamos):**
- IP do PC mudava a cada rede → app fala com a API pelo **cabo USB** (`adb reverse`).
- Conflito de merge na padronização do design → resolvido arquivo a arquivo + novo build.
- Auditoria de segurança grande → **8 fases**, cada uma com testes (inclusive tentativas de burlar).
- Não dava tempo para tudo → **PO priorizou** UX e segurança e replanejou o resto, registrado no backlog.

**Fala:** "O desafio mais prático foi o celular físico não achar a API cada vez que mudávamos de rede; resolvemos pelo cabo USB. O mais importante foi de prioridade: não cabia tudo, então escolhemos deixar o app seguro e usável antes de criar telas novas."

### Slide 11 — Próximos passos até o EF (10/12) · Luis · 40 s
**Tela:**
1. Homologação completa em celulares reais (QR real, 2 porteiros, garçom + cozinha).
2. Contador adulto/criança + lotação.
3. Cronograma com avisos.
4. Atalho do gerente.
5. WhatsApp em 1 toque e ocorrências com foto.
6. Modo offline da portaria.
7. Aviso de privacidade no app, SMTP de produção e build final.

**Fala:** "Até a entrega final, primeiro fechamos o que ficou do Épico 2 e a homologação, depois WhatsApp, fotos e modo offline, e terminamos com o build final."

### Slide 12 — Encerramento · Luis · 15 s
**Tela:** "Celebri Staff — a festa inteira na palma da mão da equipe." · links dos repositórios · Perguntas?

**Fala:** "Obrigado! A documentação atualizada, com backlog e camada de dados, está no README do repositório."

---

**Dicas para o pitch:**
- Os slides 3 e 9 respondem diretamente ao que o professor pediu de novo neste marco (funcionalidades de app e camada de dados). Não corram neles.
- Se der, mostrem no celular por 30 s no slide 3: ler um QR com a lanterna ligada e sentir a vibração.
- Completem a linha de "desafio de organização da equipe" no slide 10 e na seção 3.7 do README.
