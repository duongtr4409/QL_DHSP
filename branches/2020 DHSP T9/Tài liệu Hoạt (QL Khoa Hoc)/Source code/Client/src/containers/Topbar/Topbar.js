import React, { Component } from "react";
import { connect } from "react-redux";
import { Link } from "react-router-dom";
import { Layout, Drawer, Icon, Tooltip } from "antd";
import appActions from "../../redux/app/actions";
import TopbarWrapper from "./topbar.style";
// import logoWhite from "../../image/logoHNUE-white.png";
import logoWhite from "../../image/logoHNUE.png";
import logoutIcon from "../../image/logout.png";
import queryString from "query-string";
import server from "../../settings";
import Menu from "../../components/uielements/menu";
import options from "../Sidebar/options";
import CONSTANT from "../../settings/constants";
import "./topbar_style.css";
import authAction from "../../redux/auth/actions";

const { logout } = authAction;

const SubMenu = Menu.SubMenu;
const stripTrailingSlash = (str) => {
  if (str.substr(-1) === "/") {
    return str.substr(0, str.length - 1);
  }
  return str;
};
const { changeOpenKeys, changeCurrent } = appActions;

const { Header } = Layout;
const { toggleCollapsed, clearMenu } = appActions;

class Topbar extends Component {
  constructor(props) {
    super(props);
    this.state = {
      visibleDrawer: false,
    };
    this.handleClick = this.handleClick.bind(this);
    this.onOpenChange = this.onOpenChange.bind(this);
  }

  getAncestorKeys = (key) => {
    const map = {
      sub3: ["sub2"],
    };
    return map[key] || [];
  };

  handleClick(e) {
    if (e.key !== "btn-hd") {
      this.props.changeCurrent([e.key]);
      if (this.props.app.view === "MobileView") {
        setTimeout(() => {
          this.props.toggleCollapsed();
          this.props.toggleOpenDrawer();
        }, 100);
      }
    }
  }

  onOpenChange(newOpenKeys) {
    const { app, changeOpenKeys } = this.props;
    const latestOpenKey = newOpenKeys.find((key) => !(app.openKeys.indexOf(key) > -1));
    const latestCloseKey = app.openKeys.find((key) => !(newOpenKeys.indexOf(key) > -1));
    let nextOpenKeys = [];
    if (latestOpenKey) {
      nextOpenKeys = this.getAncestorKeys(latestOpenKey).concat(latestOpenKey);
    }
    if (latestCloseKey) {
      nextOpenKeys = this.getAncestorKeys(latestCloseKey);
    }
    changeOpenKeys(nextOpenKeys);
  }

  getMenuItem = ({ singleOption, submenuStyle, submenuColor }) => {
    const { key, label, leftIcon, children } = singleOption;
    const url = stripTrailingSlash(this.props.url);
    if (children) {
      return (
        <SubMenu
          key={key}
          title={
            <span className="isoMenuHolder" style={submenuColor}>
              <Icon type={leftIcon} />
              <span className="nav-text">{label}</span>
            </span>
          }
          popupClassName={"popupSubMenuInline"}
        >
          {children.map((child) => {
            const linkTo = child.withoutDashboard ? `/${child.key}` : `${url}/${child.key}`;
            return (
              <Menu.Item key={child.key}>
                <Link style={submenuColor} to={linkTo}>
                  {child.label}
                </Link>
              </Menu.Item>
            );
          })}
        </SubMenu>
      );
    }
    return (
      <Menu.Item key={key}>
        <Link to={`${url}/${key}`}>
          <span className="isoMenuHolder" style={submenuColor}>
            <Icon type={leftIcon} />
            <span className="nav-text">{label}</span>
          </span>
        </Link>
      </Menu.Item>
    );
  };

  render() {
    const { app, toggleCollapsed, url, customizedTheme, locale } = this.props;

    const collapsed = this.props.collapsed && !this.props.openDrawer;
    const userInfo = JSON.parse(localStorage.getItem("user"));
    // if (!userInfo) {
    //   window.location.reload();
    // }
    const userName = userInfo && userInfo.TenCanBo ? userInfo.TenCanBo : "Cán bộ";
    let userNameShort = userName.split(" ").slice(-2);
    userNameShort = userNameShort.join(" ");
    userNameShort = userNameShort.length > 14 ? userNameShort.substring(0, 12) + "..." : userNameShort;
    const styling = {
      backgroundColor: "#1D446B",
      position: "fixed",
      width: "100%",
      // height: 45,
      border: "none",
      overflow: "hidden",
    };

    const arrayKey = this.props.current;
    let link = "dashboard";
    if (arrayKey && arrayKey.length) {
      link = arrayKey[0];
    }

    const mode = "horizontal"; //collapsed === true ? "vertical" : "inline";
    const roleStore = localStorage.getItem("role");
    const role = JSON.parse(roleStore);
    let listOptions = [];
    options.forEach((menu) => {
      if (menu.children && menu.children.length) {
        let children = [];
        menu.children.forEach((menuChild) => {
          //if menuChild has permission
          if (role && role[menuChild.key] && role[menuChild.key].view) {
            children.push(menuChild);
          }
        });
        if (children.length) listOptions.push({ ...menu, children });
      } else {
        if (role && role[menu.key] && role[menu.key].view) {
          listOptions.push(menu);
        }
      }
    });
    const submenuStyle = {
      backgroundColor: "rgba(0,0,0,0.3)",
      color: customizedTheme.textColor,
    };
    const submenuColor = {
      color: customizedTheme.textColor,
    };
    return (
      <TopbarWrapper style={{ userSelect: "none" }}>
        {/*<Header className={"topBarInfo"}>*/}
        {/*  <Link to={"/dashboard"}>*/}
        {/*    <img src={logoWhite} alt={"logo"} />*/}
        {/*    <div className={"textInfoTopBar"}>*/}
        {/*      <b style={{ fontSize: 20 }}>TRƯỜNG ĐẠI HỌC SƯ PHẠM HÀ NỘI</b>*/}
        {/*      <br />*/}
        {/*      <span style={{ fontSize: 18 }}>PHẦN MỀM QUẢN LÝ NGHIÊN CỨU KHOA HỌC</span>*/}
        {/*    </div>*/}
        {/*  </Link>*/}
        {/*  <div className={"actionInfoTopBar"}>*/}
        {/*    <div className={"userTopBar"}>*/}
        {/*      <Tooltip title={userName.length > 15 ? userName : ""}>*/}
        {/*        <div className={"welcomeText"}>Xin chào! {userNameShort}</div>*/}
        {/*      </Tooltip>*/}
        {/*      <div className={"logoutDiv"}>*/}
        {/*        <Tooltip title={"Đăng xuất"}>*/}
        {/*          <img src={logoutIcon} alt={""} onClick={this.props.logout} />*/}
        {/*        </Tooltip>*/}
        {/*      </div>*/}
        {/*    </div>*/}
        {/*  </div>*/}
        {/*</Header>*/}
        <Header style={styling} className={`${collapsed ? "isomorphicTopbar collapsed" : "isomorphicTopbar"} topBarInfo`}>
          <div className="isoLeft">
            <button
              className={
                collapsed ? "triggerBtn menuCollapsed" : "triggerBtn menuOpen"
              }
              onClick={toggleCollapsed}
            />
            <Link to={"/dashboard"}>
              <img src={logoWhite} alt={"logo"} className={'logo'}/>
              <div className={"textInfoTopBar"}>
                <b className={'text-info-big'}>PHẦN MỀM QUẢN LÝ KHOA HỌC VÀ CÔNG NGHỆ</b>
                <br />
                <span className={'text-info-small'}>Công ty Cổ phần Hệ thống 2B</span>
              </div>
            </Link>
            <div className={"actionInfoTopBar"}>
              <div className={"userTopBar"}>
                <Tooltip title={userName.length > 14 ? userName : ""}>
                  <div className={"welcomeText"}>Xin chào! {userNameShort}</div>
                </Tooltip>
                <div className={"logoutDiv"}>
                  <Tooltip title={"Đăng xuất"}>
                    <img src={logoutIcon} alt={""} onClick={this.props.logout} />
                  </Tooltip>
                </div>
              </div>
            </div>
            {/*<Menu*/}
            {/*  onClick={this.handleClick}*/}
            {/*  theme="dark"*/}
            {/*  className="triggerIsoDashboardMenu"*/}
            {/*  mode={mode}*/}
            {/*  //openKeys={collapsed ? [] : app.openKeys}*/}
            {/*  selectedKeys={app.current && app.current.length && app.current[0] !== "dashboard" ? app.current : ["item_0"]} //###########dashboard*/}
            {/*  onOpenChange={(newOpenKeys) => this.onOpenChange(newOpenKeys, app)}*/}
            {/*>*/}
            {/*  {listOptions.map((singleOption) => this.getMenuItem({ submenuStyle, submenuColor, singleOption }))}*/}
            {/*</Menu>*/}
          </div>
        </Header>
      </TopbarWrapper>
    );
  }
}

export default connect(
  (state) => ({
    ...state.App,
    app: state.App,
    locale: state.LanguageSwitcher.language.locale,
    customizedTheme: state.ThemeSwitcher.topbarTheme,
  }),
  { toggleCollapsed, clearMenu, changeOpenKeys, changeCurrent, logout }
)(Topbar);
