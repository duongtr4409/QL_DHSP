import React, {Component} from "react";
import {connect} from "react-redux";
import clone from "clone";
import {Link} from "react-router-dom";
import {Layout, Icon} from "antd";
import options from "./options";
import Scrollbars from "../../components/utility/customScrollBar.js";
import Menu from "../../components/uielements/menu";
import SidebarWrapper from "./sidebar.style";
import appActions from "../../redux/app/actions";
import Logo from "../../components/utility/logo";
import {store} from "../../redux/store";
import CONSTANT from "../../settings/constants";
import {object} from "prop-types";

const SubMenu = Menu.SubMenu;
const {Sider} = Layout;

const {toggleOpenDrawer, changeOpenKeys, changeCurrent, toggleCollapsed} = appActions;
const stripTrailingSlash = (str) => {
  if (str.substr(-1) === "/") {
    return str.substr(0, str.length - 1);
  }
  return str;
};

class Sidebar extends Component {
  constructor(props) {
    super(props);
    this.handleClick = this.handleClick.bind(this);
    this.onOpenChange = this.onOpenChange.bind(this);
  }

  handleClick(e) {
    this.getNewRoleID(e.key);
    const {app} = this.props;
    this.props.changeCurrent([e.key]);
    const current = this.getRootPath(app.current[0]);
    const newKey = this.getRootPath(e.key);
    if (current === newKey) {
      window.location.reload();
    } else {
      if (window.innerWidth <= CONSTANT.COMPUTER_SIZE) {
        setTimeout(() => {
          this.props.toggleCollapsed();
        }, 100);
      }
    }
  }

  getRootPath = (key) => {
    const arr = key.split("-");
    arr.splice(arr.length - 1, 1);
    return arr.join("-");
  };

  getNewRoleID = (key) => {
    const arr = key.split("-");
    const roleID = arr[arr.length - 1];
    localStorage.setItem('role_id', roleID);
  };

  onOpenChange(newOpenKeys) {
    const {app, changeOpenKeys, toggleCollapsed} = this.props;
    const collapsed = clone(app.collapsed);
    if (collapsed) {
      toggleCollapsed();
    }
    const latestOpenKey = newOpenKeys.find((key) => !(app.openKeys.indexOf(key) > -1));
    const latestCloseKey = app.openKeys.find((key) => !(newOpenKeys.indexOf(key) > -1));
    let nextOpenKeys = [];
    if (latestOpenKey) {
      if (latestOpenKey.includes('quan-tri-he-thong')) {
        nextOpenKeys = [latestOpenKey].concat(app.openKeys);
      } else {
        nextOpenKeys = [latestOpenKey];
      }
    }
    if (latestCloseKey) {
      nextOpenKeys = newOpenKeys;
    }
    changeOpenKeys(nextOpenKeys);
  }

  getMenuItem = ({singleOption, submenuColor}, role, index) => {
    const {key, label, leftIcon, children, showMenu} = singleOption;
    const url = stripTrailingSlash(this.props.url);
    if (children) {
      console.log(89,children)
      return (
        <SubMenu
          key={`${key}-${role.RoleID}`}
          title={
            <span className="isoMenuHolder" style={submenuColor}>
              <Icon type={leftIcon}/>
              <span className="nav-text">{label}</span>
            </span>
          }
          popupClassName={"popupSubMenuInline"}
          style={{paddingLeft: '5px !important'}}
        >
          {children.map((child) => {
            let param = ``;
            const linkTo = child.withoutDashboard ? `/${child.key}` : `${url}/${child.key}${param}`;
            return (
              <Menu.Item key={`${child.key}-${role.RoleID}`}>
                <Link style={submenuColor} to={linkTo}>
                  {child.label} 
                </Link>
              </Menu.Item>
            );
          })}
        </SubMenu>
      );
    }
    if (showMenu) {
      const user = JSON.parse(localStorage.getItem('user'));
      const CanBoID = user.CanBoID;
      const CoQuanID = user.CoQuanID;
      let param = ``;
      if (key === 'chi-tiet-nha-khoa-hoc') {
        param += `?CanBoID=${CanBoID}&CoQuanID=${CoQuanID}`;
      }
      return (
        <Menu.Item key={`${key}-${role.RoleID}`}>
          <Link to={`${url}/${key}${param}`}>
            <span className="isoMenuHolder" style={submenuColor}>
              <Icon type={leftIcon}/>
              <span className="nav-text">{label}</span>
            </span>
          </Link>
        </Menu.Item>
      );
    }
  };

  getListOption = (role) => {
    const {Role} = role;
    let listOptions = [];
    options.forEach((menu) => {
      if (menu.children && menu.children.length) {
        let children = [];
        menu.children.forEach((menuChild) => {
          //if menuChild has permission
          const roleFilter = Role.filter(item => item.MaChucNang === menuChild.key && item.Xem)[0];
          if (roleFilter) {
            children.push(menuChild);
          }
        });
        if (children.length) listOptions.push({...menu, children});
      } else {
        const roleFilter = Role.filter(item => item.MaChucNang === menu.key && item.Xem)[0];
        if (roleFilter) {
          listOptions.push(menu);
        }
      }
    });
    return listOptions;
  };

  getMenuItem_v2 = ({submenuColor}, role, index) => {
    const listOptions = this.getListOption(role);
    listOptions.RoleName = role.RoleName;
    return (
      <SubMenu key={role.RoleName}
               title={
                 <span className="isoMenuHolder" style={submenuColor}>
                    <Icon type={'pic-left'}/>
                    <span className="nav-text">{role.RoleName}</span>
                 </span>}
      >
        {listOptions.map((singleOption) => this.getMenuItem({submenuColor, singleOption}, role, index))}
      </SubMenu>
    )
  };

  render() {
    const {app, customizedTheme, height, toggleCollapsed} = this.props;
    const collapsed = clone(app.collapsed);
    const mode = collapsed === true ? "vertical" : "inline";
    const styling = {
      backgroundColor: customizedTheme.backgroundColor,
    };
    const submenuStyle = {
      backgroundColor: "rgba(0,0,0,0.3)",
      color: customizedTheme.textColor,
    };
    const submenuColor = {
      color: customizedTheme.textColor,
    };
    //get list option
    // const roleStore = localStorage.getItem("role");
    // const role = JSON.parse(roleStore);
    const listRole = JSON.parse(localStorage.getItem("list_role"));
    // let listOptions = [];
    // options.forEach((menu) => {
    //   if (menu.children && menu.children.length) {
    //     let children = [];
    //     menu.children.forEach((menuChild) => {
    //       //if menuChild has permission
    //       if (role && role[menuChild.key] && role[menuChild.key].view) {
    //         children.push(menuChild);
    //       }
    //     });
    //     if (children.length) listOptions.push({...menu, children});
    //   } else {
    //     if (role && role[menu.key] && (menu.key === 'ql-nha-khoa-hoc' || menu.key === 'chi-tiet-nha-khoa-hoc')) {
    //       if (menu.key === 'ql-nha-khoa-hoc') {
    //         listOptions.push(menu);
    //       }
    //       if (menu.key === 'chi-tiet-nha-khoa-hoc') {
    //         const isQL = listOptions.filter(item => item.key === 'ql-nha-khoa-hoc');
    //         if (!isQL || isQL.length === 0) {
    //           menu.label = 'Lý lịch khoa học';
    //           listOptions.push(menu);
    //         }
    //       }
    //     } else if (role && role[menu.key] && role[menu.key].view) {
    //       listOptions.push(menu);
    //     }
    //   }
    // });
    return (
      <SidebarWrapper style={{userSelect: "none"}}>
        <Sider
          trigger={null}
          collapsible={true}
          collapsed={collapsed}
          width={200}
          className="isomorphicSidebar"
          style={styling}
          // onMouseEnter={toggleCollapsed}
        >
          <Scrollbars style={{height: height - 0}}>
            <Menu onClick={this.handleClick} theme="dark" className="isoDashboardMenu" mode={mode}
                  openKeys={collapsed ? [] : app.openKeys} selectedKeys={app.current} onOpenChange={this.onOpenChange}>
              {/*{listOptions.map((singleOption) => this.getMenuItem({submenuStyle, submenuColor, singleOption}))}*/}
              {listRole.map((role, index) => this.getMenuItem_v2({submenuColor}, role, index))}
            </Menu>
          </Scrollbars>
        </Sider>
      </SidebarWrapper>
    );
  }
}

export default connect(
  (state) => ({
    app: state.App,
    customizedTheme: state.ThemeSwitcher.sidebarTheme,
    height: state.App.height,
  }),
  {toggleOpenDrawer, changeOpenKeys, changeCurrent, toggleCollapsed}
)(Sidebar);
