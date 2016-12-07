//todo JS AQUi
var app = angular.module("appVenda", []);

//Diretiva codigo
app.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });

                event.preventDefault();
            }
        });
    };
});

//Controler
app.controller("addProduto", ['$scope', '$http', '$log', function ($scope, $http, $log) {

    $scope.produto = {};
    $scope.lista = [];

    $scope.update = function (produto) {
        $scope = angular.copy(produto);
    }

    //função de faz o calculo do total preço * quantidade
    $scope.total = function () {
        if (($scope.produto.cod != undefined) && ($scope.produto.quantidade != null)) {
            return ($scope.produto.preco * $scope.produto.quantidade);
        }
        else {
            return 0.00;
        }
    }

    //função que busca informaçoes do produto atraves do codigo com (enter).
    $scope.infor = function (c) {
        if (c == 'click') {
            $scope.update = function (produto) { $scope = angular.copy(produto); }
            $log.debug($scope.produto.cod);
            buscarInfor($scope.produto.cod);
            //document.querySelector("input[name='quantidade']").focus();//seleciona os produtos
        }
    }

    //funcão que busca informaçoes do produto atraves do Nome com (enter).
    $scope.inforNome = function (e) {
        if (e.$$watchers[2].last > 0) {
            buscarInfor(e.$$watchers[2].last);
            //document.querySelector("input[name='codigo']").value = 5555;
        }
    }

    //infor busca produtos atraves do nome 
    $scope.produtosList = function (attr) {
        $log.debug($scope.produto.nome);
        //produtoProcura/id
        if ($scope.produto.nome != undefined) {
            var BuscaAjax = $http.get("/Vendas/ProdutoProcura/" + $scope.produto.nome)
            BuscaAjax.success(function (data, status, headers, config) {
                $log.debug(data);
                $log.debug($scope.ProdutosList);
                $scope.ProdutosList = data;

                //guanbiarra JQUERY
                if (data.length > 0) {
                    $(function () {
                        $('.dropProduto').show();//.css("display", "block");

                        $('.dropProduto').focusout(function () {
                            $('.dropProduto').hide();
                        });
                        $("input[name='produto']").focusout(function () {
                            $('.dropProduto').focusout(function () {
                                $('.dropProduto').hide();
                            });
                        });
                    });
                }
                else {
                    $(function () {
                        $('.dropProduto').hide();
                    });
                }

                //guanbiarra JQUERY END                
            });
            BuscaAjax.error(function (data, status, headers, config) {
                $log.debug("Failed Connect From Server");
            });
        }
    }

    //busca informaçoes do produto
    function buscarInfor(cod) {
        if (cod != undefined) {
            var precoAjax = $http.get("/Vendas/ProdutoDetalhes/" + cod)
            precoAjax.success(function (data, status, headers, config) {
                $log.debug(data);                
                $scope.produto.cod = parseInt(data.codigo);
                $scope.produto.nome = data.nome;
                $scope.produto.preco = data.preco;
                $scope.produto.max = data.quantidade;
                //bota um if aqui
                //Produto Inexistente
                if ($scope.produto.nome != "Produto Inexistente") {
                    document.querySelector("input[name='quantidade']").focus();
                }
            });
            precoAjax.error(function (data, status, headers, config) {
                $log.debug("Failed Connect From Server");
            });
        }
    }

    //function pra adiconar itens a lista
    $scope.addItem = function () {
        function add() {
            if (($scope.produto.quantidade <= $scope.produto.max) && ($scope.produto.cod != undefined) && ($scope.produto.nome != undefined) && ($scope.produto.quantidade > 0)) {
                var date = new Date();
                var mseg = date.getHours() + "" + date.getMinutes() + "" + date.getMilliseconds();
                $log.warn(mseg);
                var produtoItem = {
                    codigo: $scope.produto.cod,
                    nome: $scope.produto.nome,
                    valorUnitario: $scope.produto.preco,
                    quantidade: $scope.produto.quantidade,
                    total: $scope.total(),
                    id: mseg
                }
                $scope.lista.push(produtoItem);

                $log.debug(produtoItem);

                $scope.produto.cod = null;
                $scope.produto.nome = null;
                $scope.produto.preco = null;
                $scope.produto.quantidade = null;
                $scope.produto.max = null;
                document.querySelector("input[name='codigo']").focus();
            }
        }

        if ($scope.lista.length > 0) {
            var crt = true;
            for (var j = 0; j < $scope.lista.length; j++) {
                if ($scope.lista[j].codigo == $scope.produto.cod) {
                    crt = false;
                    break;
                }
            }
            if (crt) {
                add();
            }
            else {
                $scope.produto.nome = "este produto ja esta na lista";
            }
        }
        else {
            add();
        }
    }

    //fuction remover item da lista
    $scope.delItem = function (e) {
        //
        var iden = e.$$watchers[0].last;
        for (var i = 0; i < $scope.lista.length; i++) {
            if ($scope.lista[i].id == iden) {
                $scope.lista.splice(i, 1);
            }
        }
    }

    //Função responsavel por finalizar a  venda
    $scope.terminaVenda = function () {
        var listasDeItensAJAX = [];

        $log.debug($scope.lista);

        for (var ii = 0; ii < $scope.lista.length; ii++) {
            $log.debug("codigo = " + $scope.lista.codigo);
            $log.debug("quantidade = " + $scope.lista.quantidade);
            itemProduto = {
                COD_PRODUTO: $scope.lista[ii].codigo,
                QUANTIDADE: $scope.lista[ii].quantidade
            };
            listasDeItensAJAX.push(itemProduto);
        }

        $log.debug("json");
        $log.debug(listasDeItensAJAX);
        var PostDate = JSON.stringify(listasDeItensAJAX);
        $log.debug(PostDate);

        if (listasDeItensAJAX.length > 0) {
            $log.debug("RESPOSTA");
            $http.post("/Vendas/FinalizaVenda/", PostDate)
            .success(function (data, status, headers, config) {
                $scope.LimpaVenda();
                document.querySelector("input[name='codigo']").focus();
                $scope.produto.nome = "Venda Finalizada";
            })
            .error(function (data, status, headers, config) {
                $log.debug("AJAX ERROR")
                $log.debug("Failed Connect From Server " + status);
                $log.debug(data);
                $log.debug(headers);
            });
        }
    }

    //Limpa os itens da lista de compra
    $scope.LimpaVenda = function () {
        if (confirm("Deseja Limpar Todos os itens da Lista ?")) {
            $scope.produto.cod = null;
            $scope.produto.nome = null;
            $scope.produto.preco = null;
            $scope.produto.quantidade = null;
            $scope.produto.max = null;
            //$scope.lista = null;
            $scope.lista.splice(0, $scope.lista.length);
            document.querySelector("input[name='codigo']").focus();
        }
    }

    //função responsavel pela soma dos produtos / quant
    $scope.compra = {};
    $scope.compra.soma = function () {
        var totalDaCompra = 0;
        for (var i = 0; i < $scope.lista.length; i++) {
            totalDaCompra += $scope.lista[i].total;
        }
        return totalDaCompra;
    }
    $scope.compra.quant = function () {
        return ($scope.lista.length > 0) ? $scope.lista.length : null;
    }
}]);
