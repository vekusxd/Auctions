import { Menu, Flex } from "antd";
import { useState } from "react";
import { Outlet, Link } from "react-router";
import CreateLotModal from "./CreateLotModal";

const items = [
  {
    label: <Link to={"active"}>Активные лоты</Link>,
    key: "activeAuctions",
  },
  {
    label: <Link to={"endingSoon"}>Скоро заканчиваются</Link>,
    key: "endingSoon",
  },
  {
    label: <Link to={"newArrivals"}>Новые лоты</Link>,
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
      <Flex>
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
        <CreateLotModal />
      </Flex>

      <Outlet />
    </>
  );
};

export default LotsLayout;
