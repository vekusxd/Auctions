import { useEffect, useState, useContext } from "react";
import { useParams } from "react-router";
import { AuthContext } from "../Auth/AuthContext";
import { Flex, Typography, Image, Button, message } from "antd";
import { useNavigate } from "react-router";
import CenterSpinner from "../CenterSpinner";

const { Text, Title } = Typography;

const LotDetails = () => {
  const { id } = useParams();
  const [lot, setLot] = useState(null);
  const { accessToken, tryRefresh } = useContext(AuthContext);
  const navigate = useNavigate();

  useEffect(() => {
    console.log("useEffect triggered"); // Проверка, что useEffect вызывается

    async function fetchLotDetails() {
      try {
        console.log("Fetching lot details..."); // Проверка, что функция вызывается
        let token = accessToken;

        if (!token) {
          console.log("No token, trying to refresh...");
          token = await tryRefresh();
          if (!token) {
            console.log("Refresh failed, redirecting to sign-in");
            navigate("/sign-in");
            return;
          }
        }

        console.log("Token:", token); // Проверка токена
        const response = await fetch(`/api/lots/${id}`, {
          headers: { Authorization: `Bearer ${token}` },
        });

        if (response.status === 401) {
          console.log("Unauthorized, redirecting to sign-in");
          navigate("/sign-in");
          return;
        }

        if (!response.ok) {
          throw new Error("Failed to fetch lot details");
        }

        const json = await response.json();
        console.log("Lot details:", json); // Проверка данных
        setLot(json);
      } catch (error) {
        console.error("Error fetching lot details:", error);
      }
    }

    fetchLotDetails();
  }, [accessToken, id, navigate, tryRefresh]);

  if (!lot) {
    return <CenterSpinner />;
  }

  return (
    <Flex justify={"center"} gap={"large"} style={{ marginTop: "2rem" }}>
      <Image src={lot.imgUrl} alt={lot.title} width={400} />
      <Flex vertical style={{ marginLeft: "2rem" }}>
        <Title>Лот - {lot.title}</Title>
        <Text style={{ fontSize: "1.5rem" }}>{lot.currentPrice} ₽</Text>
        <Text style={{ fontSize: "1.1rem", marginTop: "2rem" }}>
          {lot.lotCategory.title}
        </Text>
        {lot.description && (
          <>
            <Title level={4}>Описание</Title>
            <Text>{lot.description}</Text>
          </>
        )}
        <Button
          type={"primary"}
          style={{ width: "fit-content", marginTop: "1rem" }}
          onClick={async (e) => {
            e.stopPropagation();
            e.preventDefault();
            let token = accessToken;

            if (!token) {
              token = await tryRefresh();
              if (!token) {
                navigate("/sign-in");
                return;
              }
            }
            const result = await fetch("/api/bid", {
              method: "POST",
              headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer: ${token}`,
              },
              body: JSON.stringify({
                LotId: id,
              }),
            });

            if (result.status === 409) {
              message.error("Вы создатель лота!");
              return;
            }

            if (result.status === 400) {
              message.error("Вы сделали последнюю ставку!");
              return;
            }

            if (result.status === 200) {
              setLot({
                ...lot,
                currentPrice: lot.currentPrice + lot.priceStep,
              });
            }
          }}
        >
          Сделать ставку
        </Button>
      </Flex>
    </Flex>
  );
};

export default LotDetails;
