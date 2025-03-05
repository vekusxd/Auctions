import { Card } from "antd";
import Placeholder from "../Assets/placeholder.jpg";
const { Meta } = Card;
const LotCard = ({ title, price }) => (
  <Card
    hoverable
    style={{
      width: 260,
    }}
    cover={<img alt="example" src={Placeholder} />}
  >
    <Meta
      title={<div style={{ whiteSpace: "pre-wrap" }}>{title}</div>}
      description={`₽ ${price}`}
    />
  </Card>
);
export default LotCard;
