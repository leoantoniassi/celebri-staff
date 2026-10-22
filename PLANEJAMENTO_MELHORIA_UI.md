# Planejamento de Melhoria da UI — CelebriStaff

**Data:** 21/10/2026
**Escopo:** Aplicativo .NET MAUI (`CelebriStaff/`) — Views, estilos, tema white-label e consistência visual.

---

## 1. Diagnóstico atual

Análise dos arquivos (`Colors.xaml`, `Styles.xaml`, `App.xaml` e Views em `CelebriStaff/Views/`) indicou os seguintes problemas:

| # | Problema | Evidência |
|---|----------|-----------|
| 1 | **Estilos inline repetidos** em todas as páginas (fontes, paddings, cores) | `LoginPage.xaml`, `HomePage.xaml`, etc. definem `FontSize`, `FontAttributes`, `Padding` inline |
| 2 | **Cores hardcoded** fora do dicionário de cores (quebra o tema white-label) | `HomePage.xaml:33` (`Stroke="#DDDDDD"`), `:54` (`#B3261E`) |
| 3 | **Poucos estilos reutilizáveis** | `Styles.xaml` contém apenas `Headline` e `SubHeadline`; o resto é template default do MAUI |
| 4 | **Duas famílias de cores concorrentes** | `ColorPrimary/Secondary/Tertiary` (white-label) vs `Primary`/`Secondary` default do template (#512BD4 roxo) — risco de páginas usarem a chave errada |
| 5 | **Feedback visual fraco** | Erros exibidos como `Label` vermelho simples (`LoginPage.xaml:33`); sem toasts/snackbars; botões sem estados pressed/disabled diferenciados |
| 6 | **Cards sem hierarquia visual** | Card de escala na Home usa apenas texto empilhado; status sem badge/chip |
| 7 | **Espaçamento e alinhamento inconsistentes** | Paddings variados (24, 16, 14, 4) sem escala padronizada |
| 8 | **Estados vazios e de carregamento** | `IsEmpty`/`EmptyView` não utilizados na `CollectionView` da Home |
| 9 | **Acessibilidade** | Sem contraste auditado (amarelo #FEDC57 com texto branco), sem `SemanticProperties`, sem-bandas de toque de 44px+ |

## 2. Objetivos

1. Padronizar a identidade visual em todas as telas via `ResourceDictionary`.
2. Garantir que o tema white-label (`ThemeService.Apply` + `DynamicResource`) funcione em 100% da UI.
3. Melhorar feedback ao usuário (loading, erros, estados vazios).
4. Aumentar acessibilidade e conforto de toque.
5. Reduzir XAML duplicado (manutenibilidade).

## 3. Design System proposto

### 3.1 Tokens de cor (Colors.xaml)

- Consolidar: manter **somente** as chaves white-label (`ColorPrimary`, `ColorSecondary`, `ColorTertiary`) + escala de neutros.
- Adicionar chaves semânticas (todas via `DynamicResource`):

```
ColorBackground / ColorSurface / ColorSurfaceAlt
ColorTextPrimary / ColorTextSecondary
ColorOnPrimary (texto sobre botão primário)
ColorError / ColorSuccess / ColorWarning
ColorBorder / ColorShadow
```

- Remover as cores default do template MAUI (`Primary`, `Magenta`, `MidnightBlue`...) após migrar os usos.

### 3.2 Tipografia

| Estilo | FontSize | FontAttributes |
|--------|----------|----------------|
| `TitleText` | 22 | Bold |
| `Headline` (existente) | ajustar | Bold |
| `BodyText` | 15 | — |
| `CaptionText` | 12 | — |
| `StatusChipText` | 12 | Bold |

### 3.3 Espaçamento

Escala única em `Styles.xaml`: `SpacingXS=4`, `SpacingS=8`, `SpacingM=16`, `SpacingL=24` (via `Thickness` resources) — substituir paddings/margins arbitrários.

### 3.4 Controles (Styles.xaml)

Criar estilos implícitos/explícitos:

- `PrimaryButtonStyle` — fundo `ColorPrimary`, texto `ColorOnPrimary`, canto 12, cor de pressed/disabled.
- `SecondaryButtonStyle` (outline/danger) — substituir o `BorderColor="#B3261E"` inline.
- `InputStyle` — Entry com placeholder cinza, altura 44+, borda de foco na cor primária.
- `CardStyle` — Border com raio 14, stroke `ColorBorder`, sombra leve.
- `StatusBadgeStyle` — chip colorido por status da escala.
- `SectionHeaderStyle` — títulos de grupo da Home.

### 3.5 Componentes reutilizáveis

- `Views/Controls/RatingBadge`? Não aplicável. Em vez disso:
- **Card de Escala** como `ContentView` reutilizável (gabarito único p/ Home e módulos).
- **EmptyStateView** (ícone + título + mensagem + botão "Recarregar").
- **ToastServiceProvider** ou uso de `CommunityToolkit.Maui` (Snackbar/Toast) para erros e confirmações.

## 4. Escopo por tela (Épicos → Stories)

### Épico 1 — Fundações de estilo (chave para todo o resto)
- E1.1 Consolidar `Colors.xaml` com tokens semânticos p/ white-label.
- E1.2 Criar estilos de botão, entry, card, badge em `Styles.xaml`.
- E1.3 Definir escala de espaçamento e tipografia.
- E1.4 Auditar `ThemeService.Apply` — garantir sobrescrita de todas as chaves usadas.

### Épico 2 — Fluxo de entrada
- E2.1 `LoginPage`: logo + nome em destaque, card central, inputs estilizados, botão primário com estado "carregando", erro em box destacado.
- E2.2 `PrimeiroAcessoPage` e `EntradaConvitePage`: mesmo layout-padrão de formulário.

### Épico 3 — Home / Escalas
- E3.1 Substituir card inline por componente `EscalaCard`; status como badge colorido.
- E3.2 Adicionar `EmptyView` (listas vazia) e skeleton/loading inicial.
- E3.3 Padronizar cabeçalho de grupos e header de página (`Shell`).

### Épico 4 — Módulos operacionais (Garçom, Cozinha, Mesa, Portaria, QR)
- E4.1 Migrar todos os usos de cor hardcoded para tokens `DynamicResource`.
- E4.2 Padronizar toolbars/botões de ação (tamanho de toque ≥ 44px).
- E4.3 `LeitorQrPage`: moldura de leitura claramente visível, feedback de sucesso/erro.

### Épico 5 — Acessibilidade e polimento
- E5.1 Auditoria de contraste (fundo claro/escuro, texto sobre `ColorPrimary`).
- E5.2 `SemanticProperties` (descrições em botões de ícone, leitor de QR).
- E5.3 Modo escuro: definir variantes das chaves via `AppThemeBinding`.
- E5.4 Animações sutis (transição de páginas do Shell, no Visual Studio preview + testes em device).

## 5. Cronograma sugerido (2 sprints)

| Sprint | Entregas |
|--------|----------|
| Sprint 1 | Épico 1 completo + Épico 2 (Login/PrimeiroAcesso/Convite) |
| Sprint 2 | Épicos 3, 4 e 5 |

## 6. Critérios de aceite

- Nenhuma cor hex hardcoded nas Views (verificado por grep: `#\w{6}` em `.xaml` de Views).
- Todas as cores de marca usam `DynamicResource` (o tema do buffet troca em runtime sem reiniciar o app).
- Todos os botões têm estilos definidos no dicionário e estados visuais (pressed/disabled).
- Listas possuem estado vazio e indicador de carregamento.
- Contraste mínimo AA (WCAG 4.5:1) em textos e botões, verificado com ferramenta (ex.: Contrast Checker).
- Build Android/Windows sem regressões e fluxo do `TESTE_MANUAL.md` validado.

## 7. Riscos

| Risco | Mitigação |
|-------|-----------|
| Troca de chave de cor quebrar white-label em runtime | Testes manuais com `ThemeService.Apply` em cada tela |
| Muitas páginas alteradas em pouca hora | Migrar tela por tela, com commit isolado por tela |
| Contraste do amarelo #FEDC57 com texto branco falha | Definir `ColorOnPrimary` escuro (ex.: #24201A) no token |
| Estilos afetando telas não migradas (estilos implícitos globais) | Usar estilos com `x:Key` explícitos, aplicando gradualmente |

## 8. Métricas de sucesso

- Redução de XAML duplicado: `-60%` em linhas de estilo inline.
- Zero ocorrência de cores fora do dicionário de tokens.
- Feedback qualitativo dos staff (garçom/cozinha/portaria) sobre clareza dos botões de ação.
