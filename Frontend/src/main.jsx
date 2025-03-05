import { createRoot } from "react-dom/client";
import { BrowserRouter } from "react-router";
import "@ant-design/v5-patch-for-react-19";
import "./index.css";
import App from "./App.jsx";

createRoot(document.getElementById("root")).render(
  <BrowserRouter>
    <App />
  </BrowserRouter>
);
