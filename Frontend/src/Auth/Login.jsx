import { Button, Form, Input } from "antd";
import Cookies from "js-cookie";
import { useNavigate } from "react-router";

const Login = () => {
  const navigate = useNavigate();
  const onFinish = async (values) => {
    const result = await fetch("https://localhost:7061/api/account/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(values),
    });
    if (result.status !== 200) {
      navigate("/sign-up");
      return;
    }
    const tokens = await result.json();

    const expires = (tokens.expiresIn * 60 || 15 * 60) * 1000;
    const expiresIn = new Date(new Date().getTime() + expires);

    Cookies.set("ACCESS_TOKEN", tokens.accessToken, { expires: expiresIn });
    Cookies.set("REFRESH_TOKEN", tokens.refreshToken);

    navigate("/");
  };
  const onFinishFailed = (errorInfo) => {
    console.log("Failed:", errorInfo);
  };

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        height: "100vh",
      }}
    >
      <div
        style={{
          padding: "3rem",
          boxShadow: "0 4px 8px rgba(0, 0, 0, 0.1)",
          borderRadius: "8px",
          backgroundColor: "#fff",
        }}
      >
        <Form
          name="basic"
          labelCol={{
            span: 8,
          }}
          wrapperCol={{
            span: 16,
          }}
          style={{
            maxWidth: 400,
          }}
          initialValues={{
            remember: true,
          }}
          onFinish={onFinish}
          onFinishFailed={onFinishFailed}
          autoComplete="off"
        >
          <Form.Item
            label="Email"
            name="Email"
            rules={[
              {
                required: true,
                type: "email",
                message: "Please input your Email!",
              },
            ]}
          >
            <Input />
          </Form.Item>

          <Form.Item
            label="Password"
            name="Password"
            rules={[
              {
                required: true,
                message: "Please input your password!",
              },
            ]}
          >
            <Input.Password />
          </Form.Item>

          <Form.Item wrapperCol={{ offset: 8, span: 16 }}>
            <Button type="primary" htmlType="submit">
              Login
            </Button>
          </Form.Item>
        </Form>
      </div>
    </div>
  );
};
export default Login;
