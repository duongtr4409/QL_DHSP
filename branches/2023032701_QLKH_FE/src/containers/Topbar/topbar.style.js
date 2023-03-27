import styled from 'styled-components';
import { palette } from 'styled-theme';
import { transition, borderRadius, boxShadow } from '../../settings/style-util';
import WithDirection from '../../settings/withDirection';
import CONSTANT from '../../settings/constants';

const TopbarWrapper = styled.div`
  .isomorphicTopbar {
    min-width: 475px;
    display: flex;
    justify-content: space-between;
    background-color: #ffffff;
    border-bottom: 1px solid rgba(0, 0, 0, 0.1);
    padding: ${props => props['data-rtl'] === 'rtl' ? '0 15px 0 31px' : '0 10px 0 5px'};
    z-index: 1000;
    ${transition()};

    @media only screen and (max-width: 767px) {
      //padding: ${props => props['data-rtl'] === 'rtl' ? '0px 15px 0px 15px !important' : '0px 15px 0px 15px !important'};
    }

    &.collapsed {
      //padding: ${props => props['data-rtl'] === 'rtl' ? '0 15px 0 31px' : '0 31px 0 15px'};
      @media only screen and (max-width: 767px) {
        //padding: ${props => props['data-rtl'] === 'rtl' ? '0px 15px !important' : '0px 15px !important'};
      }
    }

    .isoLeft {
      display: flex;
      align-items: center;

      @media only screen and (max-width: 767px) {
        margin: ${props => props['data-rtl'] === 'rtl' ? '0 0 0 20px' : '0 20px 0 0'};
      }
      
      .triggerHeader {
        color: white;
        display: inline-block;
        font-weight: 700;
      }
      
      img {
        display: inline-block;
      }
      
      .triggerIsoDashboardMenu {
        background: none;
        line-height: 45px !important;
      }
      
      .triggerBtn {
        width: 24px;
        height: 100%;
        align-items: center;
        justify-content: center;
        background-color: transparent;
        color: white;
        border: 0;
        outline: 0;
        position: relative;
        cursor: pointer;
        padding-right: 31px;

        &:before {
          content: '\f20e';
          font-family: 'Ionicons';
          font-size: 26px;
          color: inherit;
          line-height: 0;
          position: absolute;
        }
        @media only screen and (max-width: 767px) {
            display: -webkit-inline-flex;
            display: -ms-inline-flex;
            display: inline-flex;
        }
      }
    }  
  }
  
  .topBarInfo {
    line-height: 25px;   
      
    .textInfoTopBar {
        color: white;
        float: left;
        margin: 10px 5px;
    } 
    
    .text-info-big {
        font-size: 20px;
    }
    
    .text-info-small {
        font-size: 18px;
    }
      
    .logo {
      width: 60px;
      float: left; 
      margin: 5px;     
    }
    
    @media only screen and (max-width: 850px) {
        .text-info-big {
            font-size: 14px;
        }
    
        .text-info-small {
            font-size: 12px;
        }
    }
    
    @media only screen and (max-width: 750px) {
        .text-info-big {
            font-size: 13px;
        }
    
        .text-info-small {
            font-size: 11px;
        }
    }      
    
    @media only screen and (max-width: 700px) {
        .textInfoTopBar {
            display: none;
        }
    }        
    
    .actionInfoTopBar {
        position: fixed;
        top: 10px;
        right: 40px;
        
        .userTopBar {
            background: #FFFFFF;
            border-radius: 20px;
            height: 40px;
            line-height: 40px;
            padding: 0 10px;
            
            .welcomeText {
                font-weight: bold;
                color: #125993;
                font-size: 15px;
                float: left;
                border-right: solid 1px #125993;
                padding-right: 15px;
                max-width: 200px;
            }
            
            .logoutDiv {
                float: right;
                padding: 0 8px;
                
                img {
                    cursor: pointer;
                    width: 25px;
                    margin: 9px 0 9px 5px;
                }
            }
        }
    }
    
    // @media only screen and (max-width: 780px) {
    //     .actionInfoTopBar {
    //         top: 65px;
    //         left: 20px;
    //         max-width: 270px;
    //     }
    // }    
  }
  
  // @media only screen and (max-width: 780px) {
  //       .topBarInfo {
  //           height: 110px;
  //       }
  //   }    
`;

export default WithDirection(TopbarWrapper);
