import { Outlet, useLocation, useNavigate } from "react-router";
import Icon from "@ant-design/icons";
import { Menu } from "antd";
import { useEffect, useState } from "react";
import RegisterIconSource from "../assets/account-plus.svg?react";
import LoginIconSource from "../assets/login.svg?react";

export const RegisterIcon = (props) => (
  <Icon component={RegisterIconSource} {...props} />
);
export const LoginIcon = (props) => (
  <Icon component={LoginIconSource} {...props} />
);

const items = [
  {
    label: "Вход",
    key: "sign-in",
    icon: <LoginIcon />,
  },
  {
    label: "Регистрация",
    key: "sign-up",
    icon: <RegisterIcon />,
  },
];

const AuthLayout = () => {
  const [current, setCurrent] = useState("sign-in");  
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    setCurrent(location.pathname.substring(1));
  }, [location]);

  const onClick = (e) => {
    setCurrent(e.key);
    navigate(e.key);
  };

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        flexDirection: "column",
        height: "100vh",
      }}
    >
      <div>
        <Menu
          className="auth-menu"
          onClick={onClick}
          selectedKeys={[current]}
          mode="horizontal"
          items={items}
          style={{ justifyContent: "space-around" }}
        />

        <div
          style={{
            padding: "2rem",
            boxShadow: "0 4px 8px rgba(0, 0, 0, 0.1)",
            borderRadius: "8px",
            backgroundColor: "#fff",
            borderBottom: "10px solid blue",
          }}
        >
          <Outlet />
        </div>
      </div>
    </div>
  );
};

export default AuthLayout;
