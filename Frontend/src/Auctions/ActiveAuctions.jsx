import { Flex, Typography } from "antd";
import { useNavigate, useSearchParams } from "react-router";
import { useEffect, useState } from "react";
import { getAccessToken } from "../Auth/AuthLogic";
import LotCard from "./LotCard";

const { Title } = Typography;

const ActiveAuctions = () => {
  const [searchParams] = useSearchParams();
  const search = searchParams.get("search");
  const navigate = useNavigate();
  const [lots, setLots] = useState([]);
  useEffect(() => {
    async function fetchData() {
      const token = getAccessToken();
      const response = await fetch(
        "https://localhost:7061/api/lots?PageSize=10",
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      if (response.status === 401) {
        navigate("/sign-in");
      }
      const json = await response.json();
      setLots(json);
      console.log(json);
    }
    fetchData();
  }, [navigate]);
  console.log(search);
  return (
    <>
      <Title level={3}>Active Auctions</Title>
      <Flex wrap gap={"large"}>
        {lots.map((item, index) => (
          <LotCard
            key={index}
            title={item.title}
            price={item.currentPrice}
            img={item.imgUrl}
          />
        ))}
      </Flex>
    </>
  );
};

export default ActiveAuctions;
