import { Route, Routes } from "react-router";
import AppLayout from "./AppLayout.jsx";
import MyBids from "./MyBids.jsx";
import MySales from "./MySales.jsx";
import LotsLayout from "./Auctions/LotsLayout.jsx";
import Lots from "./Auctions/Lots.jsx";
import HomePage from "./Home.jsx";
import Login from "./Auth/Login.jsx";
import Register from "./Auth/Register.jsx";
import AuthLayout from "./Auth/AuthLayout.jsx";
import LotDetails from "./Auctions/LotDetails.jsx";

const App = () => (
  <Routes>
    <Route path="/" element={<AppLayout />}>
      <Route index element={<HomePage />} />
      <Route path="myBids" element={<MyBids />} />
      <Route path="mySales" element={<MySales />} />
      <Route path="auctions" element={<LotsLayout />}>
        <Route index element={<Lots />} />
        <Route path=":id" element={<LotDetails />} />
      </Route>
    </Route>
    <Route element={<AuthLayout />}>
      <Route path="/sign-in" element={<Login />} />
      <Route path="/sign-up" element={<Register />} />
    </Route>
  </Routes>
);

export default App;
