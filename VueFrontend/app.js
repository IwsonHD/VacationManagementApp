const app = Vue.createApp({
  //  template: '<h2>I am the template</h2>'
  data(){
    return{
        title: 'XDDDDD',
        author: 'Iwo',
        age: '21',
        showBooks: true,
        showText: 'unshow'
    }
  },
  methods: {
    changeTitle(title){
        this.title = title
      },
    switchShowBooks(){
        if(this.showBooks){
            this.showBooks = false
            this.showText = 'show'
        }else{
            this.showBooks = true
            this.showText = 'unshow'
        }
    }
  }
})

app.mount("#app")

