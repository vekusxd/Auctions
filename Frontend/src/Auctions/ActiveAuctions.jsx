import { Flex, Typography } from "antd";
import { useNavigate, useSearchParams } from "react-router";
import { useEffect, useState, useContext } from "react";
import LotCard from "./LotCard";
import { AuthContext } from "../Auth/AuthContext";
import CenterSpinner from "../CenterSpinner";

const { Title } = Typography;

const ActiveAuctions = () => {
  const [searchParams] = useSearchParams();
  const search = searchParams.get("search");
  const navigate = useNavigate();
  const [lots, setLots] = useState([]);
  const { accessToken, tryRefresh } = useContext(AuthContext);

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
      let token = accessToken;

      if (!token) {
        token = await tryRefresh();
        if (!token) {
          navigate("/sign-in");
          return;
        }
      }

      const searchParams = new URLSearchParams();
      searchParams.append("PageSize", "20");
      searchParams.append("search", search ?? "");

      const response = await fetch(`/api/lots?${searchParams.toString()}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      if (response.status === 401) {
        navigate("/sign-in");
      }
      const json = await response.json();
      setLots(json);
    }
    fetchData();
  }, [accessToken, navigate, search, tryRefresh]);

  if (!lots.length) {
    return <CenterSpinner />;
  }

  return (
    <>
      <Title level={3}>Активные аукционы</Title>
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

export default ActiveAuctions;
