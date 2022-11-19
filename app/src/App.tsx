import { useState } from "react";
import reactLogo from "./assets/react.svg";
import { invoke } from "@tauri-apps/api/tauri";
import ServicesStatus from "./components/ServicesStatus";
import Search from "./components/Search";

function App() {
  // const [greetMsg, setGreetMsg] = useState("");
  // const [name, setName] = useState("");

  // async function greet() {
  //   // Learn more about Tauri commands at https://tauri.app/v1/guides/features/command
  //   setGreetMsg(await invoke("greet", { name }));
  // }

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
