import styled from "styled-components";
import { palette } from "styled-theme";
import { transition, borderRadius } from "../../settings/style-util";
import WithDirection from "../../settings/withDirection";
import Dang from "../../image/logoHNUE-white.png";

const SidebarWrapper = styled.div`
.isomorphicSidebar {
  z-index: 999;
  flex: 0 0 200px;
  margin-top: 64px;
}

.scrollarea {
  height: calc(100vh - 70px);
}

.isoDashboardMenu {
  padding-top: 15px;
  background: transparent;
}

.ant-menu-inline.ant-menu-sub {
  background: transparent;
}

.ant-menu-inline > .submenu-v1 > .ant-menu-submenu-title {
  padding-left: 16px !important;
  margin-left: 16px;
  border-radius: 12px;
  width: 80%;
  height: 48px;
}

.isoMenuHolder {
  line-height: 48px;
}

.icon-sidebar {
  padding: 6px;
  color: #fff;
  border-radius: 4px;
  background-color: #ccc;
}

.icon-sidebar.active-icon-sidebar {
  color: #fff;
  background-color: rgb(24, 144, 255);
}

.ant-menu-sub.ant-menu-inline > .item-sidebar,
.ant-menu-inline > .submenu-v2 > .ant-menu-submenu-title {
  width: 80%;
  height: 48px!important;
  padding-left: 16px !important;
  margin-left: 32px;
  border-radius: 12px;
}

.submenu-v2 {
  margin-bottom: 12px;
}

.active-boxshadow-sidebar {
  background-color: #fff;
  box-shadow: 0 20px 27px rgb(0 0 0/5%);
  transition: all 0.3s;
}

.active-boxshadow-sidebar span {
  font-size: 1.05em;
  font-weight: 600;
}

.ant-menu-item.active-boxshadow-sidebar {
  width: 85%;
  height: 48px;
  line-height: 48px;
  padding-left: 16px !important;
  margin-left: 36px;
  border-radius: 12px;
  transition: all 0.1s;
}

.ant-menu-inline .ant-menu-item {
  transition: all 0.1s;
}

.ant-menu-inline .ant-menu-item:not(:last-child) {
  margin-bottom: 0px !important;
}

.ant-menu-inline .ant-menu-item:last-child {
  margin-bottom: 12px;
}

.active-boxshadow.submenu-v1 > 
.ant-menu-submenu-title[aria-expanded="true"] {
  background-color: #fff;
  box-shadow: 0 20px 27px rgb(0 0 0/5%);
  transition: all 0.3s;
}

.active-boxshadow.submenu-v1 > 
.ant-menu-submenu-title[aria-expanded="true"] span {
  font-size: 1.05em;
  font-weight: 600;
}

.active-boxshadow.submenu-v1 > 
.ant-menu-submenu-title[aria-expanded="true"] span i {
  color: #fff;
  background-color: rgb(24, 144, 255);
}


.active-boxshadow.submenu-v2 > 
.ant-menu-submenu-title[aria-expanded="true"] {
  background-color: #fff;
  box-shadow: 0 20px 27px rgb(0 0 0/5%);
  transition: all 0.3s;
}

.active-boxshadow.submenu-v2 > .ant-menu-submenu-title[aria-expanded="true"] span {
  font-size: 1.05em;
  font-weight: 600;
}

.active-boxshadow.submenu-v2 > .ant-menu-submenu-title[aria-expanded="true"] span i {
  color: #fff;
  background-color: rgb(24, 144, 255);
}
`;

// `
//   .isomorphicSidebar {
//     z-index: 999;
//     background: ${palette('secondary', 0)};
//     width: 200px;
//     flex: 0 0 200px;
//     margin-top: 64px;
//     .scrollarea {
//       height: calc(100vh - 70px);
//     }

//     @media only screen and (max-width: 767px) {
//       width: 200px !important;
//       flex: 0 0 210px !important;
//     }

//     &.ant-layout-sider-collapsed {
//       @media only screen and (max-width: 767px) {
//         width: 0;
//         min-width: 0 !important;
//         max-width: 0 !important;
//         flex: 0 0 0 !important;
//       }
//     }

//     .isoLogoWrapper {
//       height: 70px;
//       background: rgba(0, 0, 0, 0.3);
//       margin: 0;
//       padding: 0 10px;
//       text-align: center;
//       overflow: hidden;
//       ${borderRadius()};

//       h3 {
//         a {
//           font-size: 21px;
//           ///font-weight: 300;
//           line-height: 70px;
//           ///letter-spacing: 3px;
//           text-transform: uppercase;
//           color: ${palette('grayscale', 6)};
//           display: block;
//           text-decoration: none;
//         }
//       }
//     }

//     &.ant-layout-sider-collapsed {
//       .isoLogoWrapper {
//         padding: 0;

//         background-image: url(${Dang});
//         background-size: 40px;
//         background-repeat: no-repeat;
//         background-position: center;

//         h3 {
//           a {
//             font-size: 27px;
//             font-weight: 500;
//             letter-spacing: 0;
//           }
//         }
//       }
//       flex: 0 0 50px !important;
//       max-width: 50px !important;
//       min-width: 50px !important;
//       width: 50px !important;
//     }

//     .isoDashboardMenu {
//       padding-top: 15px;
//       padding-bottom: 35px;
//       background: transparent;

//       a {
//         text-decoration: none;
//         font-weight: 400;
//       }

//       .ant-menu-item {
//         width: 100%;
//         display: -ms-flexbox;
//         display: flex;
//         -ms-flex-align: center;
//         align-items: center;
//         padding-left: 10px !important;
//         margin: 0;
//       }

//       .isoMenuHolder {
//         display: flex;
//         align-items: center;

//         i {
//           font-size: 19px;
//           color: inherit;
//           margin: ${props =>
//           props['data-rtl'] === 'rtl' ? '0 0 0 30px' : '0 15px 0 0'};
//           width: 18px;
//           ${transition()};
//         }
//       }

//       .anticon {
//         font-size: 18px;
//         margin-right: 30px;
//         color: inherit;
//         ${transition()};
//       }

//       .nav-text {
//         font-size: 14px;
//         color: inherit;
//         //font-weight: 400;
//         ${transition()};
//       }

//       .ant-menu-item-selected {
//         background-color: rgba(0, 0, 0, 0.4) !important;
//         .anticon {
//           color: #fff;
//         }

//         i {
//           color: #fff;
//         }

//         .nav-text {
//           color: #fff;
//         }
//       }

//       .ant-menu-item:hover{
//         background-color: rgba(0, 0, 0, 0.4) !important;
//       }

//       > li {
//         &:hover {
//           i,
//           .nav-text {
//             color: #ffffff;
//           }
//         }
//       }
//     }

//     .ant-menu-dark .ant-menu-inline.ant-menu-sub {
//       background: ${palette('secondary', 5)};
//     }

//     .ant-menu-submenu-inline,
//     .ant-menu-submenu-vertical {
//       > .ant-menu-submenu-title {
//         width: 100%;
//         display: flex;
//         align-items: center;
//         padding-left: 10px !important;

//         > span {
//           display: flex;
//           align-items: center;
//         }

//         .ant-menu-submenu-arrow {
//           opacity: 1;
//           left: ${props => (props['data-rtl'] === 'rtl' ? '25px' : 'auto')};
//           right: ${props => (props['data-rtl'] === 'rtl' ? 'auto' : '10px')};

//           &:before,
//           &:after {
//             background: #DCDCDC;
//             width: 8px;
//             ${transition()};
//           }

//           &:before {
//             transform: rotate(-45deg) translateX(3px);
//           }

//           &:after {
//             transform: rotate(45deg) translateX(-3px);
//           }
//         }
//       }

//       .ant-menu-inline,
//       .ant-menu-submenu-vertical {
//         > li:not(.ant-menu-item-group) {
//           padding-left: ${props =>
//           props['data-rtl'] === 'rtl' ? '0px !important' : '15px !important'};
//           padding-right: ${props =>
//           props['data-rtl'] === 'rtl' ? '15px !important' : '0px !important'};
//           font-size: 13px;
//           font-weight: 400;
//           margin: 0;
//           color: inherit;
//           ${transition()};

//           &:hover {
//             a {
//               color: #ffffff !important;
//             }
//           }
//         }

//         .ant-menu-item-group {
//           padding-left: 0;

//           .ant-menu-item-group-title {
//             padding-left: 100px !important;
//           }
//           .ant-menu-item-group-list {
//             .ant-menu-item {
//               padding-left: 125px !important;
//             }
//           }
//         }
//       }

//       .ant-menu-submenu-inline .ant-menu-inline > li:not(.ant-menu-item-group) {
//         padding-left: 0px !important;
//       }

//       .ant-menu-sub {
//         box-shadow: none;
//         background-color: transparent !important;
//       }
//     }

//     &.ant-layout-sider-collapsed {
//       .nav-text {
//         display: none;
//       }

//       .ant-menu-submenu-inline >  {
//         .ant-menu-submenu-title:after {
//           display: none;
//         }
//       }

//       .ant-menu-submenu-vertical {
//         > .ant-menu-submenu-title:after {
//           display: none;
//         }

//         .ant-menu-sub {
//           background-color: transparent !important;

//           .ant-menu-item {
//             height: 35px;
//           }
//         }
//       }
//     }

//     .ant-menu-submenu {
//       .ant-menu-submenu {
//         .ant-menu-submenu-title {
//           padding-left: 0px !important;
//         }
//       }
//     }
//   }
// `;

export default WithDirection(SidebarWrapper);
