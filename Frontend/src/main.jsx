import { createRoot } from "react-dom/client";
import { BrowserRouter } from "react-router";
import "@ant-design/v5-patch-for-react-19";
import "./index.css";
import App from "./App.jsx";
import { AuthProvider } from "./Auth/AuthProvider.jsx";
import { StrictMode } from "react";

createRoot(document.getElementById("root")).render(
  <StrictMode>
    <BrowserRouter>
      <AuthProvider>
        <App />
      </AuthProvider>
    </BrowserRouter>
  </StrictMode>
);
