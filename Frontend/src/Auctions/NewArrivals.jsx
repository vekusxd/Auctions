import { Flex, Typography } from "antd";
import { useNavigate } from "react-router";
import { useEffect, useState } from "react";
import { getAccessToken } from "../Auth/AuthLogic";
import LotCard from "./LotCard";

const { Title } = Typography;

const NewArrivals = () => {
  const navigate = useNavigate();
  const [lots, setLots] = useState([]);

  const updateLot = (id, newPrice, newBidCount) => {
    setLots(
      lots.map((l) => {
        if (l.id === id) {
          return { ...l, currentPrice: newPrice, numberOfBids: newBidCount };
        } else {
          return l;
        }
      })
    );
  };

  useEffect(() => {
    async function fetchData() {
      const token = getAccessToken();

      const response = await fetch("/api/lots/active", {
        headers: { Authorization: `Bearer ${token}` },
      });
      if (response.status === 401) {
        navigate("/sign-in");
      }
      const json = await response.json();
      setLots(json);
    }
    fetchData();
  }, [navigate]);

  return (
    <>
      <Title level={3}>Новые аукционы</Title>
      <Flex wrap gap={"large"}>
        {lots.map((item, index) => (
          <LotCard
            key={index}
            id={item.id}
            title={item.title}
            price={item.currentPrice}
            img={item.imgUrl}
            updateLot={updateLot}
            step={item.priceStep}
            bidsCount={item.numberOfBids}
          />
        ))}
      </Flex>
    </>
  );
};

export default NewArrivals;
