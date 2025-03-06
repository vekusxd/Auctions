import { Card } from "antd";
import PropTypes from "prop-types";
const { Meta } = Card;
const LotCard = ({ title, price, img }) => (
  <Card
    hoverable
    style={{
      width: 260,
    }}
    cover={<img alt="example" src={img} />}
  >
    <Meta
      title={<div style={{ whiteSpace: "pre-wrap" }}>{title}</div>}
      description={`₽ ${price}`}
    />
  </Card>
);
export default LotCard;

LotCard.propTypes = {
  title: PropTypes.string.isRequired,
  price: PropTypes.number.isRequired,
  img: PropTypes.string.isRequired,
};
