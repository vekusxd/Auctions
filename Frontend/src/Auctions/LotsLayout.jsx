import { Menu, Flex } from "antd";
import { useEffect, useState, useContext } from "react";
import { Outlet } from "react-router";
import { useNavigate } from "react-router";
import { AuthContext } from "../Auth/AuthContext";
import CreateLotModal from "./CreateLotModal";

const items = [
  {
    label: "Активные лоты",
    key: "activeAuctions",
  },
  {
    label: "Скоро заканчиваются",
    key: "endingSoon",
  },
  {
    label: "Новые лоты",
    key: "newArrivals",
  },
];

const LotsLayout = () => {
  const [current, setCurrent] = useState("activeAuctions");
  const [categories, setCategories] = useState([]);
  const [dateSort, setDateSort] = useState(0);
  const navigate = useNavigate();
  const { accessToken, tryRefresh } = useContext(AuthContext);

  useEffect(() => {
    const fetchCategories = async () => {
      let token = accessToken;

      if (!token) {
        token = await tryRefresh();
        if (!token) {
          navigate("/sign-in");
          return;
        }
      }

      const result = await fetch("/api/lotCategories?PageSize=30", {
        headers: {
          Authorization: "Bearer " + token,
        },
      });
      if (result.status === 401) {
        navigate("/sign-in");
      }
      const json = await result.json();
      const data = json.map((item) => {
        return { value: item.id, label: item.title };
      });
      setCategories(data);
    };
    fetchCategories();
  }, [accessToken, navigate, tryRefresh]);

  const onClick = (e) => {
    if (e.key === "endingSoon") {
      setDateSort(0);
    } else {
      setDateSort(1);
    }
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
        <CreateLotModal categories={categories} />
      </Flex>

      <Outlet context={[categories, dateSort]} />
    </>
  );
};

export default LotsLayout;
