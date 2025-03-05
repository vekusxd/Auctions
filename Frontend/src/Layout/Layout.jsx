import {Layout, Menu, theme} from 'antd';
import {Outlet} from "react-router";

const {Header, Content, Footer} = Layout;

const items = Array.from({
    length: 15,
}).map((_, index) => ({
    key: index + 1, label: `nav ${index + 1}`,
}));
const MyLayout = () => {
    const {
        token: {colorBgContainer, borderRadiusLG},
    } = theme.useToken();
    return (<Layout style={{minHeight: "100vh"}}>
        <Header
            style={{
                display: 'flex', alignItems: 'center',
            }}
        >
            <div className="demo-logo"/>
            <Menu
                theme="dark"
                mode="horizontal"
                defaultSelectedKeys={['2']}
                items={items}
                style={{
                    flex: 1, minWidth: 0,
                }}
            />
        </Header>
        <Content
            style={{
                padding: '0 48px',
                flex: "1",
                display: "flex"
            }}>
            <div
                style={{
                    background: colorBgContainer,
                    height: "100%",
                    padding: 24,
                    flex: "1",
                    borderRadius: borderRadiusLG}}>
            </div>
        </Content>
        <Footer
            style={{
                textAlign: 'center',
            }}>
            Ant Design ©{new Date().getFullYear()} Created by Ant UED
        </Footer>
    </Layout>);
};
export default MyLayout;