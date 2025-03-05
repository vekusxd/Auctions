import { Route, Routes } from "react-router";
import AppLayout from "./AppLayout.jsx";
import MyBids from "./MyBids.jsx";
import MySales from "./MySales.jsx";
import LotsLayout from "./Auctions/LotsLayout.jsx";
import Active from "./Auctions/Active.jsx";
import EndingSoon from "./Auctions/EndingSoon.jsx";
import NewArrivals from "./Auctions/NewArrivals.jsx";
import Lots from "./Auctions/ActiveAuctions.jsx";
import HomePage from "./Home.jsx";
import Login from "./Auth/Login.jsx";
import Register from "./Auth/Register.jsx";

const App = () => (
  <Routes>
    <Route path="/" element={<AppLayout />}>
      <Route index element={<HomePage />} />
      <Route path="myBids" element={<MyBids />} />
      <Route path="mySales" element={<MySales />} />
      <Route path="auctions" element={<LotsLayout />}>
        <Route index element={<Lots />} />
        <Route path="active" element={<Active />} />
        <Route path="endingSoon" element={<EndingSoon />} />
        <Route path="newArrivals" element={<NewArrivals />} />
      </Route>
    </Route>
    <Route path="/sign-in" element={<Login />} />
    <Route path="/sign-up" element={<Register />} />
  </Routes>
);

export default App;
