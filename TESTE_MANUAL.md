# Cronograma de teste manual — Celebri + Celebri Staff

Roteiro para validar as 3 etapas (login/escala, portaria, garçom/cozinha) no
painel web e no app. São 5 sessões, na ordem abaixo: cada uma prepara os
dados da seguinte. Tempo total estimado: **~2h**.

Marque `[x]` em cada item. Se algo não sair como o **Esperado**, anote a
sessão, o número do item e o que apareceu na tela.

| Sessão | O quê | Onde | Tempo | Pessoas/aparelhos |
| :-- | :-- | :-- | :-- | :-- |
| 0 | Preparar o ambiente | PC | 15 min | 1 |
| 1 | Configurar o evento no painel | Web | 30 min | 1 (gerente) |
| 2 | Login e escala no app | App | 20 min | 1 celular/emulador |
| 3 | Portaria | App + Web | 20 min | 1 celular **com câmera** (+1 opcional) |
| 4 | Garçom e cozinha | App | 30 min | 2 aparelhos (ou 1 alternando) |
| 5 | Casos de erro e segurança | Web + App | 15 min | 1 |

---

## Sessão 0 — Preparar o ambiente (15 min)

1. [ ] No repositório `celebri`: `docker compose up -d --build` e depois `docker compose exec backend npm run seed`.
   - **Esperado:** `http://localhost:3001/api/health` responde `success: true`; o log da API mostra as migrations até `011_garcom_cozinha.sql` aplicadas.
2. [ ] Abrir o painel em `http://localhost:8080` e entrar com `gerente@celebri.com` / `123456`.
3. [ ] Rodar o app (`dotnet build -t:Run -f net10.0-android`) no emulador **e**, para a sessão 3, num celular Android real (a câmera do emulador não lê QR de verdade). No celular, troque o endereço da API em `Services/Api/AppConfig.cs` pelo IP do PC na rede.
4. [ ] Sem SMTP configurado no `.env`, o código de primeiro acesso aparece no log: deixe aberto `docker compose logs -f backend`.

---

## Sessão 1 — Configurar o evento no painel web (30 min)

**Funções e intervalo**
1. [ ] Cadastros → Funções: abra **Garçom**, **Cozinheiro** e **Recepcionista**.
   - **Esperado:** campo "Tela no app Celebri Staff" já vem como Garçom, Cozinha e Portaria; a lista mostra o selo "App: …".
2. [ ] Crie a função "Copeiro" com tela **Cozinha** e salve.
3. [ ] Configurações → Escalas: mude o intervalo mínimo para **1** hora e salve.
   - **Esperado:** mensagem "Intervalo entre escalas atualizado!". Recarregue a página: continua 1.

**Funcionários (conta do app)**
4. [ ] Funcionários → Novo: cadastre 3 pessoas **com e-mail** (ex.: `porteiro@teste.com` Recepcionista, `garcom@teste.com` Garçom, `cozinha@teste.com` Cozinheiro).
   - **Esperado:** cadastro normal. (A conta do app é criada sozinha, sem senha.)

**Evento de hoje**
5. [ ] Eventos → Novo: crie um evento **hoje**, começando daqui a ~1h e terminando em ~5h, com local **Salão 1** e 50 pessoas (30 adultos, 20 crianças).
6. [ ] Selecione o evento → **Criar Escala**: escale os 3 funcionários.
7. [ ] **Ver Escala**: para o garçom, troque a "função neste evento" para **Recepcionista** e depois volte para **Padrão**.
   - **Esperado:** cada pessoa aparece com "Aguardando resposta"; a troca salva sem recarregar a página.
8. [ ] Crie um segundo evento hoje que comece 1h30 depois do fim do primeiro e tente escalar o garçom.
   - **Esperado:** permitido (intervalo 1h). Volte o intervalo para 2h em Configurações e tente de novo com outra pessoa: bloqueado com "intervalo mínimo de 2h".

**Convidados**
9. [ ] No evento de hoje → **Convidados**: ligue "Usar lista de convidados com QR code".
10. [ ] Adicione "Família Souza" (4 pessoas, com seu WhatsApp) e "Carlos Lima" (1 pessoa).
    - **Esperado:** os dois aparecem com "entraram 0"; os números do topo somam 5 pessoas em 2 convites.
11. [ ] Clique no ícone de QR da Família Souza.
    - **Esperado:** abre a página do convite (sem login) com nome do buffet, evento, QR e "Válido para 4 pessoas".
12. [ ] Clique no ícone do WhatsApp.
    - **Esperado:** abre o WhatsApp com a mensagem e o link do QR.

**Mesas e cardápio**
13. [ ] Cadastros → Locais → ícone de mesa no **Salão 1**: clique em 4 pontos vazios do salão.
    - **Esperado:** surgem as mesas 1, 2, 3 e 4 onde você clicou.
14. [ ] Clique na mesa 4, mude para 8 lugares, salve; com ela selecionada, clique em outro ponto do salão.
    - **Esperado:** a mesa muda de lugar. "Desmarcar" volta ao modo de criar.
15. [ ] No evento → **Cardápio**: adicione "Salgados fritos · 600 un · 19:30", "Mini pizzas · 200 un · 20:00" e "Bolo · 1 un" (sem horário).
    - **Esperado:** lista ordenada por horário, bolo por último, todos "Pendente".

---

## Sessão 2 — Login e escala no app (20 min)

1. [ ] Na tela de e-mail, digite `porteiro@teste.com` → Continuar.
   - **Esperado:** o app aplica as cores do buffet e pede o código de 6 dígitos.
2. [ ] Digite um código **errado** + senha.
   - **Esperado:** "Código inválido ou expirado".
3. [ ] Pegue o código certo no log da API, crie a senha `teste123` (e confirme).
   - **Esperado:** entra direto em "Minhas Escalas" com o evento de hoje.
4. [ ] Toque **Confirmar**.
   - **Esperado:** "Presença confirmada"; só o botão Recusar continua visível. No painel web (Ver Escala), aparece "Confirmou".
5. [ ] Toque **Cheguei no evento**.
   - **Esperado:** "Chegou às HH:MM"; no painel aparece o mesmo horário.
   - Se o evento começar em mais de 3h, o botão não aparece: é o comportamento esperado.
6. [ ] Feche o app sem sair e abra de novo.
   - **Esperado:** volta direto para "Minhas Escalas".
7. [ ] **Sair** → entre com `porteiro@teste.com` de novo.
   - **Esperado:** agora pede a **senha** (não o código), com o e-mail já preenchido.
8. [ ] Faça o primeiro acesso também de `garcom@teste.com` e `cozinha@teste.com` (vai precisar deles na sessão 4).

---

## Sessão 3 — Portaria (20 min) · celular com câmera

Logado como `porteiro@teste.com`, na escala de hoje toque **Abrir Portaria**.

1. [ ] **Esperado:** número grande "0 pessoas presentes", "de 50 previstos", botão de QR e a lista com os 2 convites.
2. [ ] Toque **+1 entrou** três vezes e **−1** uma vez.
   - **Esperado:** total 2, "2 sem convite".
3. [ ] **Ler QR code do convite** → permita a câmera → aponte para o QR da Família Souza (na tela do PC ou no WhatsApp).
   - **Esperado:** abre "Entrada do convidado" com "Família Souza · Convite para 4 · faltam 4" e quantidade 4.
4. [ ] Mude para **3** e **Registrar entrada**.
   - **Esperado:** volta para a portaria com total 5 e "Entraram 3 de 4".
5. [ ] Leia o mesmo QR de novo e registre **2**.
   - **Esperado:** aviso em laranja "1 pessoa(s) além do previsto"; a tela não fecha sozinha.
6. [ ] Toque **Desfazer 1 entrada**.
   - **Esperado:** volta para "já entraram 4 (completo)".
7. [ ] Busque "carl" na busca → toque em Carlos Lima → registre 1.
   - **Esperado:** total 7.
8. [ ] Leia um QR qualquer que não seja do sistema (ex.: de um site).
   - **Esperado:** "QR code inválido…" e a câmera volta a ler depois de ~2s.
9. [ ] (Opcional, 2 aparelhos) Dois porteiros tocam **+1 entrou** ao mesmo tempo várias vezes.
   - **Esperado:** em até ~8s os dois mostram o mesmo total, sem perder nenhuma entrada.
10. [ ] No painel web → Convidados: os números batem com o app.

---

## Sessão 4 — Garçom e cozinha (30 min) · 2 aparelhos

Aparelho A: `garcom@teste.com` → **Abrir Garçom**. Aparelho B: `cozinha@teste.com` → **Abrir Cozinha**.
(Com um aparelho só: faça as ações de um, saia, entre com o outro.)

**Garçom**
1. [ ] **Esperado (A):** mapa com as 4 mesas nas posições do painel, todas cinzas ("livre").
2. [ ] Toque a mesa 2 → adicione "Suco de uva" ×3 → adicione "Mini pizza" ×2 com anotação "sem glúten" e marque **Alergia** → **Enviar pedido**.
   - **Esperado:** "Pedido enviado para a cozinha!"; o pedido aparece em "Pedidos desta mesa" como "Enviado à cozinha".
3. [ ] Volte ao mapa.
   - **Esperado:** mesa 2 laranja com selo "1".

**Cozinha**
4. [ ] **Esperado (B):** aba "Pedidos (1 novo)"; card da mesa 2 com borda vermelha e "⚠ ALERGIA: 2× Mini pizza — sem glúten".
5. [ ] Toque **Começar** e depois **Pronto**.
   - **Esperado:** status muda para "Em preparo" e depois "Pronto — aguardando garçom".
6. [ ] Aba **Cardápio**: toque "Começar preparo" e depois "Marcar servido" nos salgados.
   - **Esperado:** status Pendente → Em preparo → Servido. No painel web (Cardápio) o status acompanha.
7. [ ] Aba **Estoque**: veja os produtos reservados para o evento.
   - **Esperado:** produtos no ou abaixo do mínimo aparecem primeiro com "⚠ Estoque baixo". Se nenhum produto foi reservado ao evento, aparece "Nenhum produto reservado".

**De volta ao garçom**
8. [ ] **Esperado (A, em até ~8s):** mesa 2 verde com "✓" e o resumo "1 pedido(s) pronto(s)".
9. [ ] Toque a mesa 2 → **Entreguei na mesa**.
   - **Esperado:** pedido "Entregue"; a mesa volta a cinza no mapa e o pedido some da cozinha.
10. [ ] Faça outro pedido na mesa 1 e toque **Cancelar pedido** antes da cozinha começar.
    - **Esperado:** some da tela da cozinha.

---

## Sessão 5 — Casos de erro e segurança (15 min)

1. [ ] App: e-mail que não existe → "E-mail não cadastrado em nenhum buffet".
2. [ ] App: primeiro acesso com código errado 5 vezes, depois o certo.
   - **Esperado:** continua recusando; **Reenviar código** gera um novo que funciona.
3. [ ] App: no porteiro, **Recusar** a escala de amanhã (se houver) → confirmar.
   - **Esperado:** "Você recusou"; no painel aparece "Recusou".
4. [ ] App: o garçom abre o evento de outro dia em que **não** está escalado.
   - **Esperado:** o botão de módulo só aparece no dia do evento (3h antes até o fim).
5. [ ] Web: remova o convite do Carlos Lima e tente ler o QR dele na portaria.
   - **Esperado:** "Convite não encontrado".
6. [ ] Web: tente criar outra mesa "2" no Salão 1 (editando a mesa 3 para o número 2).
   - **Esperado:** "Já existe uma mesa com esse número neste local."
7. [ ] Web: desligue "Usar lista de convidados" no evento.
   - **Esperado:** no app, a portaria esconde o botão de QR e a busca (fica só o contador) em até ~8s.

---

### Limitações conhecidas (não são bugs)
- **Atualização:** as telas do app se atualizam a cada ~8s (consulta periódica); não é tempo real.
- **Sem internet:** sem conexão o app mostra erro e não guarda entradas para enviar depois (o modo offline ficou para o backlog, US07).
- **E-mail do código:** sem SMTP no `.env`, o código só aparece no log da API.
