import PropTypes from "prop-types";
import { Spin } from "antd";

const CenterSpinner = ({ size }) => {
  return (
    <Spin
      size={size}
      style={{
        position: "absolute",
        top: "50%",
        left: "50%",
        transform: " translate(-50%, -50%)",
      }}
    />
  );
};

export default CenterSpinner;

CenterSpinner.defaultProps = {
  size: "large",
};

CenterSpinner.propTypes = {
  size: PropTypes.string,
};
