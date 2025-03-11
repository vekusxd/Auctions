import { Button, Form, Input } from "antd";
import { useNavigate } from "react-router";
import { AuthContext } from "./AuthContext";
import { useContext } from "react";

const Login = () => {
  const navigate = useNavigate();
  const { login } = useContext(AuthContext);

  const onFinish = async (values) => {
    const result = await login(values);
    if (!result) {
      navigate("/sign-in");
    } else {
      navigate("/");
    }
  };
  const onFinishFailed = (errorInfo) => {
    console.log("Failed:", errorInfo);
  };

  return (
    <Form
      name="basic"
      labelCol={{
        span: 8,
      }}
      wrapperCol={{
        span: 16,
      }}
      style={{
        maxWidth: 600,
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
            message: "Пожалуйста введите ваш email",
          },
        ]}
      >
        <Input />
      </Form.Item>

      <Form.Item
        label="Пароль"
        name="Password"
        rules={[
          {
            required: true,
            message: "Пожалуйста введите ваш пароль",
          },
        ]}
      >
        <Input.Password />
      </Form.Item>

      <Form.Item wrapperCol={{ offset: 8, span: 16 }}>
        <Button type="primary" htmlType="submit">
          Вход
        </Button>
      </Form.Item>
    </Form>
  );
};
export default Login;
