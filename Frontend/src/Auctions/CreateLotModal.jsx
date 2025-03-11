import {
  Button,
  Modal,
  Form,
  Input,
  Upload,
  InputNumber,
  DatePicker,
  Select,
  message,
} from "antd";
import { useContext, useState } from "react";
import { UploadOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router";
import { AuthContext } from "../Auth/AuthContext";
import PropTypes from "prop-types";

const CreateLotModal = ({ categories }) => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [imageUrl, setImageUrl] = useState("");
  const navigate = useNavigate();
  const [form] = Form.useForm();
  const { accessToken, tryRefresh } = useContext(AuthContext);

  const onFinish = async (values) => {
    const utcDate = new Date(Date.parse(values.EndDate)).toISOString();
    const data = {
      ...values,
      EndDate: utcDate,
      ImgUrl: values.ImgUrl.file.response.url,
    };
    if (!data.Description) {
      data.Description = "";
    }

    let token = accessToken;

    if (!token) {
      token = tryRefresh();
      if (!token) {
        navigate("/sign-in");
        return;
      }
    }

    const result = await fetch("/api/lots", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + accessToken,
      },
      body: JSON.stringify(data),
    });

    if (result.status !== 201) {
      message.error(
        `Failed to creating lot, status code: ${result.status}, message: ${result.statusText}`
      );
      console.log(result);
    } else {
      message.success("Lot created!");
      form.resetFields();
      setIsModalOpen(false);
      setImageUrl("");
    }
  };
  const onFinishFailed = (errorInfo) => {
    console.log("Failed:", errorInfo);
  };

  const props = {
    name: "file",
    action: "/api/upload",
    headers: {
      Authorization: "Bearer " + accessToken,
    },
    onChange(info) {
      if (info.file.status !== "uploading") {
        console.log(info.file, info.fileList);
      }
      if (info.file.status === "done") {
        message.success(`${info.file.name} file uploaded successfully`);
        setImageUrl(info.file.response.url);
      } else if (info.file.status === "error") {
        message.error(`${info.file.name} file upload failed.`);
      }
    },
  };

  const showModal = () => {
    setIsModalOpen(true);
  };
  const handleOk = () => {
    form.submit();
  };
  const handleCancel = () => {
    form.resetFields();
    setIsModalOpen(false);
  };

  return (
    <>
      <Button type="primary" onClick={showModal}>
        Создать новый лот
      </Button>
      <Modal
        title="Новый лот"
        okText="Создать"
        cancelText="Отмена"
        open={isModalOpen}
        onOk={handleOk}
        onCancel={handleCancel}
      >
        <Form
          form={form}
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
            label="Название"
            name="Title"
            rules={[
              {
                required: true,
                message: "Введите название лота!",
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item label="Описание" name="Description">
            <Input.TextArea
              autoSize={{
                minRows: 4,
              }}
            />
          </Form.Item>
          <Form.Item
            label="Фото"
            name="ImgUrl"
            rules={[
              {
                required: true,
                message: "Загрузите фотографию!",
              },
            ]}
          >
            <Upload {...props}>
              <Button
                disabled={imageUrl.length !== 0}
                icon={<UploadOutlined />}
              >
                загрузить
              </Button>
            </Upload>
          </Form.Item>

          <Form.Item
            label="Категория"
            name="LotCategoryId"
            rules={[
              {
                required: true,
                message: "Выберите категорию лота!",
              },
            ]}
          >
            <Select options={categories} />
          </Form.Item>
          <Form.Item
            label="Начальная цена"
            name="StartPrice"
            rules={[
              {
                required: true,
                message: "Введите начальную цену!",
              },
            ]}
          >
            <InputNumber
              addonBefore="+"
              addonAfter="₽"
              min={0}
              max={1000000}
              step={10}
            />
          </Form.Item>
          <Form.Item
            label="Шаг"
            name="PriceStep"
            rules={[
              {
                required: true,
                message: "Введите шаг ставки!",
              },
            ]}
          >
            <InputNumber
              addonBefore="+"
              addonAfter="₽"
              min={1}
              max={100000}
              step={10}
            />
          </Form.Item>
          <Form.Item
            label="Дата окончания"
            name="EndDate"
            rules={[
              {
                required: true,
                message: "Пожалуйста введите дату окончания",
              },
            ]}
          >
            <DatePicker showTime />
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
};
export default CreateLotModal;

CreateLotModal.propTypes = {
  categories: PropTypes.array,
};
