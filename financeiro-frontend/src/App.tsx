import { useEffect, useState } from "react";

const API = "http://localhost:5030/api";

/*
Esse código funciona assim:
chama o backend para carregar pessoas, categorias, transações e relatórios
mostra tudo na interface
quando o usuário cadastra algo, envia um POST para a API
se der certo, limpa o formulário e recarrega os dados
as listas e relatórios são atualizados automaticamente
*/

/* Esses tipos dizem ao TypeScript qual formato os dados devem ter. */
type Pessoa = {
  id: number;
  nome: string;
  idade: number;
};

type Categoria = {
  id: number;
  descricao: string;
  finalidade: number;
};

type Transacao = {
  id: number;
  descricao: string;
  valor: number;
  tipo: number;
  pessoaId: number;
  pessoa: string;
  categoriaId: number;
  categoria: string;
};

type TotalPorPessoa = {
  pessoaId: number;
  nome: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
};

type RelatorioPessoas = {
  pessoas: TotalPorPessoa[];
  totalGeral: {
    totalReceitas: number;
    totalDespesas: number;
    saldo: number;
  };
};

type TotalPorCategoria = {
  categoriaId: number;
  descricao: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
};

type RelatorioCategorias = {
  categorias: TotalPorCategoria[];
  totalGeral: {
    totalReceitas: number;
    totalDespesas: number;
    saldo: number;
  };
};

function App() {
  /*Esses estados guardam: os dados que aparecem na tela o que o usuário digitou nos formulários */
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [categorias, setCategorias] = useState<Categoria[]>([]);
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  const [relatorioPessoas, setRelatorioPessoas] =
    useState<RelatorioPessoas | null>(null);
  const [relatorioCategorias, setRelatorioCategorias] =
    useState<RelatorioCategorias | null>(null);

  const [nomePessoa, setNomePessoa] = useState("");
  const [idadePessoa, setIdadePessoa] = useState("");

  const [descricaoCategoria, setDescricaoCategoria] = useState("");
  const [finalidadeCategoria, setFinalidadeCategoria] = useState("1");

  const [descricaoTransacao, setDescricaoTransacao] = useState("");
  const [valorTransacao, setValorTransacao] = useState("");
  const [tipoTransacao, setTipoTransacao] = useState("1");
  const [pessoaId, setPessoaId] = useState("");
  const [categoriaId, setCategoriaId] = useState("");

  const [mensagem, setMensagem] = useState("");

/* faz uma requisição para o backend 
converte a resposta em JSON 
salva o resultado no estado */

  async function carregarPessoas() {
    const response = await fetch(`${API}/pessoas`);
    const data = await response.json();
    setPessoas(data);
  }

  async function carregarCategorias() {
    const response = await fetch(`${API}/categorias`);
    const data = await response.json();
    setCategorias(data);
  }

  async function carregarTransacoes() {
    const response = await fetch(`${API}/transacoes`);
    const data = await response.json();
    setTransacoes(data);
  }

  async function carregarRelatorioPessoas() {
    const response = await fetch(`${API}/relatorios/totais-por-pessoa`);
    const data = await response.json();
    setRelatorioPessoas(data);
  }

  async function carregarRelatorioCategorias() {
    const response = await fetch(`${API}/relatorios/totais-por-categoria`);
    const data = await response.json();
    setRelatorioCategorias(data);
  }

  async function carregarTudo() {
    await Promise.all([
      carregarPessoas(),
      carregarCategorias(),
      carregarTransacoes(),
      carregarRelatorioPessoas(),
      carregarRelatorioCategorias(),
    ]);
  }

  useEffect(() => {
    carregarTudo().catch(() => {
      setMensagem("Erro ao carregar dados do backend.");
    });
  }, []);

  async function cadastrarPessoa(e: FormEvent) {
    e.preventDefault();
    setMensagem("");

    const response = await fetch(`${API}/pessoas`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        nome: nomePessoa,
        idade: Number(idadePessoa),
      }),
    });

    if (!response.ok) {
      setMensagem("Erro ao cadastrar pessoa.");
      return;
    }

    setNomePessoa("");
    setIdadePessoa("");
    setMensagem("Pessoa cadastrada com sucesso.");
    await carregarTudo();
  }

  async function cadastrarCategoria(e: FormEvent) {
    e.preventDefault();
    setMensagem("");

    const response = await fetch(`${API}/categorias`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        descricao: descricaoCategoria,
        finalidade: Number(finalidadeCategoria),
      }),
    });

    if (!response.ok) {
      setMensagem("Erro ao cadastrar categoria.");
      return;
    }

    setDescricaoCategoria("");
    setFinalidadeCategoria("1");
    setMensagem("Categoria cadastrada com sucesso.");
    await carregarTudo();
  }

  async function cadastrarTransacao(e: FormEvent) {
    e.preventDefault();
    setMensagem("");

    const response = await fetch(`${API}/transacoes`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        descricao: descricaoTransacao,
        valor: Number(valorTransacao),
        tipo: Number(tipoTransacao),
        categoriaId: Number(categoriaId),
        pessoaId: Number(pessoaId),
      }),
    });

    if (!response.ok) {
      const erro = await response.text();
      setMensagem(`Erro ao cadastrar transação: ${erro}`);
      return;
    }

    setDescricaoTransacao("");
    setValorTransacao("");
    setTipoTransacao("1");
    setPessoaId("");
    setCategoriaId("");
    setMensagem("Transação cadastrada com sucesso.");
    await carregarTudo();
  }

  function textoTipo(tipo: number) {
    return tipo === 1 ? "Despesa" : "Receita";
  }

  function textoFinalidade(finalidade: number) {
    if (finalidade === 1) return "Despesa";
    if (finalidade === 2) return "Receita";
    return "Ambas";
  }

  return (
    <div style={styles.container}>
      <h1>Controle Financeiro</h1>
      <p style={{ color: "#0a7" }}>{mensagem}</p>

      <div style={styles.grid}>
        <section style={styles.card}>
          <h2>Cadastrar Pessoa</h2>
          <form onSubmit={cadastrarPessoa} style={styles.form}>
            <input
              placeholder="Nome"
              value={nomePessoa}
              onChange={(e) => setNomePessoa(e.target.value)}
              required
            />
            <input
              placeholder="Idade"
              type="number"
              value={idadePessoa}
              onChange={(e) => setIdadePessoa(e.target.value)}
              required
            />
            <button type="submit">Salvar Pessoa</button>
          </form>

          <h3>Lista de Pessoas</h3>
          <ul>
            {pessoas.map((p) => (
              <li key={p.id}>
                #{p.id} - {p.nome} ({p.idade} anos)
              </li>
            ))}
          </ul>
        </section>

        <section style={styles.card}>
          <h2>Cadastrar Categoria</h2>
          <form onSubmit={cadastrarCategoria} style={styles.form}>
            <input
              placeholder="Descrição"
              value={descricaoCategoria}
              onChange={(e) => setDescricaoCategoria(e.target.value)}
              required
            />
            <select
              value={finalidadeCategoria}
              onChange={(e) => setFinalidadeCategoria(e.target.value)}
            >
              <option value="1">Despesa</option>
              <option value="2">Receita</option>
              <option value="3">Ambas</option>
            </select>
            <button type="submit">Salvar Categoria</button>
          </form>

          <h3>Lista de Categorias</h3>
          <ul>
            {categorias.map((c) => (
              <li key={c.id}>
                #{c.id} - {c.descricao} ({textoFinalidade(c.finalidade)})
              </li>
            ))}
          </ul>
        </section>
      </div>

      <section style={styles.card}>
        <h2>Cadastrar Transação</h2>
        <form onSubmit={cadastrarTransacao} style={styles.form}>
          <input
            placeholder="Descrição"
            value={descricaoTransacao}
            onChange={(e) => setDescricaoTransacao(e.target.value)}
            required
          />
          <input
            placeholder="Valor"
            type="number"
            step="0.01"
            value={valorTransacao}
            onChange={(e) => setValorTransacao(e.target.value)}
            required
          />
          <select
            value={tipoTransacao}
            onChange={(e) => setTipoTransacao(e.target.value)}
          >
            <option value="1">Despesa</option>
            <option value="2">Receita</option>
          </select>

          <select
            value={pessoaId}
            onChange={(e) => setPessoaId(e.target.value)}
            required
          >
            <option value="">Selecione a pessoa</option>
            {pessoas.map((p) => (
              <option key={p.id} value={p.id}>
                {p.nome}
              </option>
            ))}
          </select>

          <select
            value={categoriaId}
            onChange={(e) => setCategoriaId(e.target.value)}
            required
          >
            <option value="">Selecione a categoria</option>
            {categorias.map((c) => (
              <option key={c.id} value={c.id}>
                {c.descricao} - {textoFinalidade(c.finalidade)}
              </option>
            ))}
          </select>

          <button type="submit">Salvar Transação</button>
        </form>
      </section>

      <section style={styles.card}>
        <h2>Lista de Transações</h2>
        <table style={styles.table}>
          <thead>
            <tr>
              <th>ID</th>
              <th>Descrição</th>
              <th>Valor</th>
              <th>Tipo</th>
              <th>Pessoa</th>
              <th>Categoria</th>
            </tr>
          </thead>
          <tbody>
            {transacoes.map((t) => (
              <tr key={t.id}>
                <td>{t.id}</td>
                <td>{t.descricao}</td>
                <td>R$ {t.valor.toFixed(2)}</td>
                <td>{textoTipo(t.tipo)}</td>
                <td>{t.pessoa}</td>
                <td>{t.categoria}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </section>

      <div style={styles.grid}>
        <section style={styles.card}>
          <h2>Totais por Pessoa</h2>
          <table style={styles.table}>
            <thead>
              <tr>
                <th>Pessoa</th>
                <th>Receitas</th>
                <th>Despesas</th>
                <th>Saldo</th>
              </tr>
            </thead>
            <tbody>
              {relatorioPessoas?.pessoas.map((p) => (
                <tr key={p.pessoaId}>
                  <td>{p.nome}</td>
                  <td>R$ {p.totalReceitas.toFixed(2)}</td>
                  <td>R$ {p.totalDespesas.toFixed(2)}</td>
                  <td>R$ {p.saldo.toFixed(2)}</td>
                </tr>
              ))}
            </tbody>
          </table>

          {relatorioPessoas && (
            <p>
              <strong>Total Geral:</strong> Receitas R${" "}
              {relatorioPessoas.totalGeral.totalReceitas.toFixed(2)} | Despesas
              R$ {relatorioPessoas.totalGeral.totalDespesas.toFixed(2)} | Saldo
              R$ {relatorioPessoas.totalGeral.saldo.toFixed(2)}
            </p>
          )}
        </section>

        <section style={styles.card}>
          <h2>Totais por Categoria</h2>
          <table style={styles.table}>
            <thead>
              <tr>
                <th>Categoria</th>
                <th>Receitas</th>
                <th>Despesas</th>
                <th>Saldo</th>
              </tr>
            </thead>
            <tbody>
              {relatorioCategorias?.categorias.map((c) => (
                <tr key={c.categoriaId}>
                  <td>{c.descricao}</td>
                  <td>R$ {c.totalReceitas.toFixed(2)}</td>
                  <td>R$ {c.totalDespesas.toFixed(2)}</td>
                  <td>R$ {c.saldo.toFixed(2)}</td>
                </tr>
              ))}
            </tbody>
          </table>

          {relatorioCategorias && (
            <p>
              <strong>Total Geral:</strong> Receitas R${" "}
              {relatorioCategorias.totalGeral.totalReceitas.toFixed(2)} |
              Despesas R$ {relatorioCategorias.totalGeral.totalDespesas.toFixed(2)} |
              Saldo R$ {relatorioCategorias.totalGeral.saldo.toFixed(2)}
            </p>
          )}
        </section>
      </div>
    </div>
  );
}

const styles: Record<string, React.CSSProperties> = {
  container: {
    maxWidth: "1200px",
    margin: "0 auto",
    padding: "24px",
    fontFamily: "Arial, sans-serif",
  },
  grid: {
    display: "grid",
    gridTemplateColumns: "1fr 1fr",
    gap: "16px",
    marginBottom: "16px",
  },
  card: {
    border: "1px solid #ddd",
    borderRadius: "8px",
    padding: "16px",
    marginBottom: "16px",
    background: "#fff",
  },
  form: {
    display: "flex",
    flexDirection: "column",
    gap: "8px",
    marginBottom: "16px",
  },
  table: {
    width: "100%",
    borderCollapse: "collapse",
  },
};

export default App;