import ServicesStatus from "./components/ServicesStatus";
import Search from "./components/Search";

function App() {
  return (
    <div id="app-container">
      <div id="left-container">
        <ServicesStatus />
      </div>
      <div id="right-container">
        <Search />
      </div>
    </div>
  );
}

export default App;
