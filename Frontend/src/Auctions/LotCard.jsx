import { Card, Typography, Flex, Button, message, Image } from "antd";
import PropTypes from "prop-types";
import { Link, useNavigate } from "react-router";
import { AuthContext } from "../Auth/AuthContext";
import { useContext } from "react";

const { Meta } = Card;

const { Title, Text } = Typography;

const LotCard = ({ id, title, price, img, updateLot, step, bidsCount }) => {
  const { accessToken, tryRefresh } = useContext(AuthContext);
  const navigate = useNavigate();
  return (
    <Card
      hoverable
      style={{
        width: 325,
        height: 425,
      }}
      cover={<Image alt="example" src={img} width={325} height={250} />}
    >
      <Link to={`/auctions/${id}`}>
        <Meta
          title={<div style={{ whiteSpace: "pre-wrap" }}>{title}</div>}
          description={
            <Flex vertical>
              <Flex justify={"space-between"}>
                <Flex vertical>
                  <Text type={"secondary"} style={{ margin: 0 }}>
                    Текущая ставка
                  </Text>
                  <Title
                    level={5}
                    style={{ marginTop: 0 }}
                  >{`₽ ${price}`}</Title>
                </Flex>
                <Text
                  style={{
                    color: "#679cde",
                    background: "#e9f3ff",
                    height: "fit-content",
                    alignSelf: "center",
                    padding: "0.1rem 0.3rem",
                    borderRadius: "5px",
                    border: "1px solid #679cde",
                  }}
                >
                  {bidsCount} ставок
                </Text>
              </Flex>

              <Button
                type={"primary"}
                onClick={async (e) => {
                  e.stopPropagation();
                  e.preventDefault();

                  let token = accessToken;

                  if (!token) {
                    token = tryRefresh();
                    if (!token) {
                      navigate("/sign-in");
                      return;
                    }
                  }

                  const result = await fetch("/api/bid", {
                    method: "POST",
                    headers: {
                      "Content-Type": "application/json",
                      Authorization: `Bearer ${token}`,
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
                    updateLot(id, price + step, bidsCount + 1);
                  }
                }}
              >
                Сделать ставку
              </Button>
            </Flex>
          }
        />
      </Link>
    </Card>
  );
};
export default LotCard;

LotCard.propTypes = {
  id: PropTypes.string.isRequired,
  title: PropTypes.string.isRequired,
  price: PropTypes.number.isRequired,
  img: PropTypes.string.isRequired,
  updateLot: PropTypes.func.isRequired,
  step: PropTypes.number.isRequired,
  bidsCount: PropTypes.number.isRequired,
};
