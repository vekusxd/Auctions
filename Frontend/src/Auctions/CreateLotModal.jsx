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
import { useEffect, useState } from "react";
import { UploadOutlined } from "@ant-design/icons";
import { getAccessToken } from "../Auth/AuthLogic";
import { useNavigate } from "react-router";

const retrieveToken = () => {
  const token = getAccessToken();
  return token;
};

const CreateLotModal = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [categories, setCategories] = useState([]);
  const [imageUrl, setImageUrl] = useState("");
  const navigate = useNavigate();
  const [form] = Form.useForm();

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

    console.log(data);

    console.log(JSON.stringify(data));

    const result = await fetch("https://localhost:7061/api/lots", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + retrieveToken(),
      },
      // body: JSON.stringify({
      //   Title: data.Title,
      //   Description: data.Description,
      //   LotCategoryId: data.LotCategoryId,
      //   StartPrice: data.StartPrice,
      //   PriceStep: data.PriceStep,
      //   EndDate: data.EndDate,
      //   ImgUrl: data.ImgUrl,
      // }),
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
      const json = await result.json();
      console.log(json);
    }
  };
  const onFinishFailed = (errorInfo) => {
    console.log("Failed:", errorInfo);
  };

  const props = {
    name: "file",
    action: "https://localhost:7061/api/upload",
    headers: {
      Authorization: "Bearer " + retrieveToken(),
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

  useEffect(() => {
    const fetchCategories = async () => {
      const token = getAccessToken();
      console.log(token);
      const result = await fetch(
        "https://localhost:7061/api/lotCategories?PageSize=30",
        {
          headers: {
            Authorization: "Bearer " + token,
          },
        }
      );
      if (result.status === 401) {
        navigate("/sign-in");
      }
      const json = await result.json();
      const data = json.map((item) => {
        return { value: item.id, label: item.title };
      });
      setCategories(data);
    };
    fetchCategories();
  }, [navigate]);

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
