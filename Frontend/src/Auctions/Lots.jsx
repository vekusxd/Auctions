import { Flex, Typography, Select } from "antd";
import { useNavigate, useSearchParams, useOutletContext } from "react-router";
import { useEffect, useState, useContext } from "react";
import LotCard from "./LotCard";
import { AuthContext } from "../Auth/AuthContext";
import CenterSpinner from "../CenterSpinner";

const { Title } = Typography;

const Lots = () => {
  const [searchParams] = useSearchParams();
  const search = searchParams.get("search");
  const [selectedCategoryId, setSelectedCategoryId] = useState("");
  const navigate = useNavigate();
  const [lots, setLots] = useState([]);
  const { accessToken, tryRefresh } = useContext(AuthContext);
  const [isLoading, setIsLoading] = useState(false);

  const [categories, dateSort] = useOutletContext();

  console.log(categories);

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
      setIsLoading(true);
      let token = accessToken;

      if (!token) {
        token = await tryRefresh();
        if (!token) {
          navigate("/sign-in");
          return;
        }
      }

      const searchParams = new URLSearchParams();
      searchParams.append("Title", search ?? "");
      searchParams.append("CategoryId", selectedCategoryId ?? "");
      searchParams.append("SortDirection", dateSort);

      const response = await fetch(`/api/lots?${searchParams.toString()}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      if (response.status === 401) {
        navigate("/sign-in");
      }
      const json = await response.json();
      setIsLoading(false);
      setLots(json);
    }
    fetchData();
  }, [accessToken, navigate, search, tryRefresh, selectedCategoryId, dateSort]);

  if (isLoading) {
    return <CenterSpinner />;
  }

  if (lots.length === 0) {
    return (
      <>
        <Select
          options={categories}
          value={selectedCategoryId}
          placeholder={"Категория лотов"}
          style={{ marginTop: "1rem", minWidth: "250px" }}
          allowClear={true}
          onChange={(val) => setSelectedCategoryId(val)}
        />
        <Title level={3}>Лотов с такой категорией нет</Title>
      </>
    );
  }

  return (
    <>
      <Select
        options={categories}
        value={selectedCategoryId}
        placeholder={"Категория лотов"}
        style={{ marginTop: "1rem", minWidth: "250px" }}
        allowClear={true}
        onChange={(val) => setSelectedCategoryId(val)}
      />
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

export default Lots;
