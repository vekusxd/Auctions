import { useEffect, useState } from "react";
import { useParams } from "react-router";
import { getAccessToken } from "../Auth/AuthLogic";
import { Flex, Typography, Image, Button, message } from "antd";

const { Text, Title } = Typography;

const LotDetails = () => {
  const { id } = useParams();
  const [lot, setLot] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const token = getAccessToken();
    const abortController = new AbortController();

    const fetchLotDetails = async () => {
      try {
        const response = await fetch(`/api/lots/${id}`, {
          headers: { Authorization: `Bearer ${token}` },
          signal: abortController.signal,
        });

        if (!response.ok) {
          throw new Error(`Ошибка запроса: ${response.statusText}`);
        }

        const json = await response.json();
        setLot(json);
      } catch (error) {
        if (error.name !== "AbortError") {
          setError(error.message);
        }
      } finally {
        setLoading(false);
      }
    };

    fetchLotDetails();

    return () => {
      abortController.abort();
    };
  }, [id]);

  if (loading) {
    return <div>Загрузка...</div>;
  }

  if (error) {
    return <div>Ошибка: {error}</div>;
  }

  if (!lot) {
    return <div>Лот не найден</div>;
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
            const result = await fetch("/api/bid", {
              method: "POST",
              headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${getAccessToken()}`,
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
