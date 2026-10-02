# Notas do apresentador — Celebri Staff — Marco 1

1. **capa** — Luis (20s): Somos a L2Tech. Vamos mostrar o Celebri Staff: de onde partimos, o que ele virou e o que entregamos no primeiro épico.
2. **ideia-inicial** — Luis (45s): Começamos com um sistema para um buffet só. Transformamos em plataforma para vários buffets, cada um com a própria marca. O app nasceu como uma lista de funções genéricas para a equipe de campo.
3. **refinamentos** — Luis (60s): Discutindo com o grupo, entendemos o que cada função faz no salão. Um exemplo: a comanda não é para cobrar, porque no buffet está tudo incluso; ela serve para o garçom pedir à cozinha. E no login, só o e-mail seria inseguro, então incluímos um código enviado por e-mail no primeiro acesso.
4. **o-que-e** — Luis (60s) — slide mais importante: Em uma frase, é o app da equipe do buffet no dia da festa. Cada funcionário entra com o e-mail, vê suas escalas e, no evento, abre a tela da sua função, sempre com a marca do buffet.
5. **escopo** — Luis (40s): Somos claros sobre o que não faremos neste semestre e sobre as limitações de hoje, que já estão no backlog.
6. **marcos** — Luis (40s): Replanejamos os épicos. Adiantamos a operação do evento, então o Épico 2 passa a validar essas telas em uso real e a completar o que falta.
7. **proposto** — Leonardo (40s): Para o Épico 1, o combinado era a base: app funcionando com a API, login que aplica a marca do buffet e a tela de escalas.
8. **realizado** — Leonardo (90s): Entregamos o proposto e fomos além. As três telas de operação já funcionam ponta a ponta com a API. Temos 256 testes automatizados no servidor e um roteiro de teste manual para homologar em celulares reais. (Se der, mostrar o app no emulador por 30 s.)
9. **jornada** — Bruno (60s): Esta é a funcionalidade principal. A Marina, recepcionista, lê o QR da Família Souza. Só o pai chegou, então ela registra 1. Quando o resto chega, o app mostra que faltam 3. Se alguém esquece o celular, ela busca pelo nome.
10. **desafios** — Leonardo (60s): O maior aprendizado foi que compilar não basta: dois bugs graves só apareceram quando rodamos no emulador. Depois disso, todo fluxo passou a ser testado contra a API de verdade.
11. **proximos** — Bruno (50s): Tratamos dados de funcionários e convidados, então aplicamos minimização e segurança desde já. Até o EP3 vamos homologar em aparelhos reais e completar contador, cronograma e UX.
12. **encerramento** — Luis (20s): Obrigado! A documentação completa está no README do repositório. Ficamos à disposição.
