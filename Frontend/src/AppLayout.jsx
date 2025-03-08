import { Layout, Menu, theme, Typography, Input, Flex, Button } from "antd";
import { useState } from "react";
import { Outlet, Link, useNavigate, createSearchParams } from "react-router";
const { Header, Content, Footer } = Layout;
const { Search } = Input;
const { Title } = Typography;

const items = [
  {
    label: <Link to={"/auctions"}>Лоты</Link>,
    key: "lots",
  },
  {
    label: <Link to={"/myBids"}>Мои ставки</Link>,
    key: "myBids",
  },
  {
    label: <Link to={"/mySales"}>Мои продажи</Link>,
    key: "mySales",
  },
];

const AppLayout = () => {
  const {
    token: { colorBgContainer, borderRadiusLG },
  } = theme.useToken();
  const [currentKey, setCurrentKey] = useState("");
  const [inputValue, setInputValue] = useState("");
  const navigate = useNavigate();
  return (
    <Layout style={{ minHeight: "100vh" }}>
      <Header
        style={{
          display: "flex",
          alignItems: "center",
          background: colorBgContainer,
          gap: "1rem",
        }}
      >
        <Title level={4} style={{ margin: "0" }}>
          <Link
            onClick={() => {
              setCurrentKey("");
            }}
            to={"/"}
            style={{ color: "black" }}
          >
            <span style={{ color: "#4a66ea" }}>Auction</span>Hub
          </Link>
        </Title>

        <Menu
          mode="horizontal"
          selectedKeys={[currentKey]}
          onClick={(e) => setCurrentKey(e.key)}
          items={items}
          style={{
            flex: 1,
            minWidth: 0,
          }}
        />

        <Flex gap={"small"}>
          <Search
            placeholder="Поиск лотов..."
            allowClear
            value={inputValue}
            onChange={(e) => setInputValue(e.target.value)}
            onSearch={(value) => {
              setCurrentKey("lots");
              navigate({
                pathname: "/auctions",
                search: createSearchParams({
                  search: value,
                }).toString(),
              });
              setInputValue("");
            }}
          />
          <Button type={"primary"}>
            <Link to={"/sign-in"}>Вход</Link>{" "}
          </Button>
          <Button>
            <Link to={"/sign-up"}>Регистрация</Link>
          </Button>
        </Flex>
      </Header>
      <Content
        style={{
          padding: "0 48px",
          flex: "1",
          display: "flex",
          marginTop: "1rem",
        }}
      >
        <div
          style={{
            background: colorBgContainer,
            minHeight: 280,
            padding: 24,
            borderRadius: borderRadiusLG,
            flex: "1",
          }}
        >
          <Outlet />
        </div>
      </Content>
      <Footer
        style={{
          textAlign: "center",
        }}
      >
        AuctionHub ©{new Date().getFullYear()} Created by Emil Batmanov
      </Footer>
    </Layout>
  );
};
export default AppLayout;
