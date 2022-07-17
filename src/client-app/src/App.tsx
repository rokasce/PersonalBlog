import "./components/styles/global-vars.css";
import { Posts } from "./components/Posts/Posts";
import { Header } from "./components/Header/Header";
import { Author } from "./components/Author/Author";

function App() {
  return (
    <div className="app">
      <Header />
      <Author />
      <Posts />
    </div>
  );
}

export default App;
