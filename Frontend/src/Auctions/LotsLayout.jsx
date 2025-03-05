import { Menu } from "antd";
import { useState } from "react";
import { Outlet, Link } from "react-router";

const items = [
  {
    label: <Link to={"active"}>Active Auctions</Link>,
    key: "activeAuctions",
  },
  {
    label: <Link to={"endingSoon"}>Ending Soon</Link>,
    key: "endingSoon",
  },
  {
    label: <Link to={"newArrivals"}>New Arrivals</Link>,
    key: "newArrivals",
  },
];

const LotsLayout = () => {
  const [current, setCurrent] = useState("activeAuctions");
  const onClick = (e) => {
    setCurrent(e.key);
  };
  return (
    <>
      <Menu
        onClick={onClick}
        selectedKeys={[current]}
        mode="horizontal"
        defaultSelectedKeys={[current]}
        items={items}
        style={{
          flex: 1,
          minWidth: 0,
        }}
      />
      <Outlet />
    </>
  );
};

export default LotsLayout;
