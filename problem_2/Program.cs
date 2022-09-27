﻿// Mesh
MeshParameters meshParameters = MeshParameters.ReadJson("MeshParameters.json");
MeshGenerator meshGenerator = new(new MeshBuilder(meshParameters));
var mesh = meshGenerator.CreateMesh();
mesh.Save("Mesh.json");

// FEM
Func<double, double, double> field = (double r, double z) => z;
Func<double, double, double> source = (double r, double z) => 0.0;

FEMBuilder femBuilder = new();
FEMBuilder.FEM fem = femBuilder.SetMesh(mesh).SetBasis(new LinearBasis()).SetSolver(new LOSLU(1000, 1e-13)).SetTest(source, field);
Console.WriteLine($"Residual: {fem.Solve()}"); 

// Electro Exploration
ElectroParameters electroParameters = ElectroParameters.ReadJson("ElectroParameters.json");
ElectroExplorationBuilder explorationBuilder = new ();
ElectroExplorationBuilder.ElectroExploration electroExploration = explorationBuilder.
                                                                  SetParameters(electroParameters).
                                                                  SetSolver(new Gauss()).
                                                                  SetFEM(fem);