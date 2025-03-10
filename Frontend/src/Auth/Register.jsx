import { Form, Input, Button, message } from "antd";
import { useNavigate } from "react-router";

const Register = () => {
  const navigate = useNavigate();
  const onFinish = async (values) => {
    console.log(values);

    const result = await fetch("api/account/register", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        FirstName: values.firstName,
        LastName: values.lastName,
        Email: values.email,
        Password: values.password,
      }),
    });
    if (result.status === 409) {
      message.error("Этот email уже зарегистрирован");
      return;
    }

    if (result.status === 400) {
      message.error("Плохой пароль!");
      return;
    }

    navigate("/sign-in");
  };

  return (
    <Form
      name="register"
      labelCol={{
        span: 9,
      }}
      wrapperCol={{
        span: 22,
      }}
      style={{
        maxWidth: 800,
      }}
      initialValues={{
        remember: true,
      }}
      autoComplete="off"
      onFinish={onFinish}
    >
      <Form.Item
        label="Имя"
        name="firstName"
        rules={[
          {
            required: true,
            message: "Введите имя",
          },
        ]}
      >
        <Input />
      </Form.Item>

      <Form.Item
        label="Фамилия"
        name="lastName"
        rules={[
          {
            required: true,
            message: "Введите фамилию",
          },
        ]}
      >
        <Input />
      </Form.Item>

      <Form.Item
        label="Email"
        name="email"
        rules={[
          {
            required: true,
            type: "email",
            message: "Введите email",
          },
        ]}
      >
        <Input />
      </Form.Item>

      <Form.Item
        label="Пароль"
        name="password"
        rules={[
          {
            required: true,
            message: "Введите пароль",
          },
        ]}
      >
        <Input.Password />
      </Form.Item>

      <Form.Item
        label="Подтверждение"
        name="confirmPassword"
        dependencies={["password"]}
        hasFeedback
        rules={[
          {
            required: true,
            message: "Подтвердите пароль",
          },
          ({ getFieldValue }) => ({
            validator(_, value) {
              if (!value || getFieldValue("password") === value) {
                return Promise.resolve();
              }
              return Promise.reject(new Error("Пароли не совпадают!"));
            },
          }),
        ]}
      >
        <Input.Password />
      </Form.Item>

      <Form.Item wrapperCol={{ offset: 8, span: 16 }}>
        <Button type="primary" htmlType="submit">
          Регистрация
        </Button>
      </Form.Item>
    </Form>
  );
};

export default Register;
