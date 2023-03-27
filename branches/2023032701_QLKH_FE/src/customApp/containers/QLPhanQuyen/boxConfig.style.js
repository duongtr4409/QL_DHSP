import styled from 'styled-components';
const BoxConfig = styled.div`
  position: relative;
  margin-bottom: 10px !important;
  .closeButton {
    position: absolute !important;
    top: -20px;
    right: -20px;
    border: none;
    background: none;
    outline: none;
    font-size: 22px;
    height: 40px;
    width: 40px;
    cursor: pointer;
    @media only screen and (max-width: 767px) {
      top: -48px;
    }
  }
  .box_class {
    padding: 15px !important;
    height: 340px;
  }
  .ant-col {
    margin-top: 28px;
  }
  h3 {
    font-size: 14px;
    position: absolute;
    left: 40px;
    top: -10px;
    background: #ffffff;
    padding: 0 5px;
  }
  .action_class {
    padding-bottom: 8px;
    button {
      margin-left: 10px;
      margin-right: 0px;
      @media only screen and (max-width: 992px) {
        margin-left: 0px;
        margin-right: 10px;
      }
    }
    @media only screen and (min-width: 992px) {
      text-align: right;
    }
  }
  .content_class {
    height: 270px;
    overflow: auto;
    button {
      border: none;
      box-shadow: none;
      background: none;
      height: auto;
      cursor: pointer;
      outline: none;
      margin-left: 10px;
    }
    button::focus, button:active{ outline: none !important }
    ul {
      padding: 20px;
      list-style-type: circle;
    }
    .content_row {
      display: flex;
      .tenchucnang {
        display: inline-block;
        width: calc(100% - 300px);
      }
      .chonxoaquyen {
        display: inline-block;
        width: 305px;
      }
    }
    .content_row:hover, li:hover {
      background: #e6f7ff;
    }
  }
  
  .content_class::-webkit-scrollbar {
    width: 4px;
    background-color: #F5F5F5;
  }
   
  .content_class::-webkit-scrollbar-track {
      background-color: #F5F5F5;
  }
   
  .content_class::-webkit-scrollbar-thumb {
    background-color: rgba(0,0,0,0.05);
  }
`;

export { BoxConfig };
